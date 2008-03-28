namespace NHibernate.Burrow.Util.Hql.Gold
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Text;
	using GoldParser;
	using NHibernate;

	/// <summary>
	/// Executes the gold parser engine (Klimstra C# Engine is used)
	/// </summary>
	public class HqlParser : IDisposable
	{
		private static readonly object syncParserReader = new object();
		private static Parser m_parser = null;

		public HqlParser()
		{
			if (m_parser == null)
			{
				lock (syncParserReader)
				{
					if (m_parser == null)
					{
						Assembly assembly = Assembly.GetExecutingAssembly();
						Stream file = assembly.GetManifestResourceStream("NHibernate.Burrow.Util.Hql.Gold.Grammar.Grammar.cgt");

						// TODO: The grammar file should be as an embedded resource (the Engine doesn't support it, but it's easy to fix with the source code)
						m_parser = new Parser(new BinaryReader(file));
					}
				}
			}
		}

		/// <summary>
		/// Executes the parser over the given text, and returns the root reduction
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Reduction Execute(string text)
		{
			bool done = false, fatal = false;
			int errors = 0, errorLine = -1;

			StringBuilder errorMessages = new StringBuilder();

			// TODO: To improve performance, the Engine should support receive text directly.
			MemoryStream memStream = new MemoryStream();
			StreamWriter writer = new StreamWriter(memStream);
			writer.Write(text);
			writer.Flush();
			memStream.Seek(0, SeekOrigin.Begin);

			m_parser.OpenStream(new StreamReader(memStream));
			m_parser.TrimReductions = true;

			while (!done && !fatal)
			{
				ParseMessage result = m_parser.Parse();

				switch (result)
				{
					case ParseMessage.TokenRead:
						// do nothing
						break;
					case ParseMessage.Reduction:
						// do nothing
						break;
					case ParseMessage.Accept:
						// program accepted
						done = true;
						break;
					case ParseMessage.LexicalError:
						errorMessages.AppendLine("LexicalError");
						fatal = true;
						break;
					case ParseMessage.SyntaxError:
						if (errorLine == (errorLine = m_parser.CurrentLineNumber))
							fatal = true; // stop if there are multiple errors on one line
						else
						{
							errors++;
							HandleSyntaxError(errorMessages);
						}
						break;
					case ParseMessage.CommentError:
						errorMessages.AppendLine("CommentError");
						fatal = true;
						break;
					case ParseMessage.InternalError:
						errorMessages.AppendLine("Internal error - this is really bad");
						fatal = true;
						break;
				}

				errors = (fatal ? errors + 1 : errors);
				fatal = fatal || (errors > 7);
			}

			if (fatal || errors > 0)
			{
				errorMessages.AppendLine(errors + " error(s)");
				errorMessages.Insert(0, "Error in query: [" + text + "]" + Environment.NewLine);

				throw new QueryException(errorMessages.ToString());
			}
			else
			{
				DumpVisitor visitor = new DumpVisitor();
				m_parser.CurrentReduction.Accept(visitor);

				if (m_parser.CurrentReduction.Tokens.Count > 0)
					return m_parser.CurrentReduction;
			}

			return null;
		}

		private void HandleSyntaxError(StringBuilder errorMessages)
		{
			int line = m_parser.CurrentLineNumber;
			TokenStack expected = m_parser.GetTokens();

			errorMessages.AppendLine("Line " + line + ": Syntax error.");
			errorMessages.AppendLine("  found:    " + m_parser.CurrentToken.Data);
			errorMessages.Append("  expected: ");

			foreach (Token token in expected)
				errorMessages.Append(token + " ");

			errorMessages.AppendLine();
			errorMessages.AppendLine();
			m_parser.PushInputToken(expected[0]);
		}

		#region IDisposable Members

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			m_parser.CloseFile();
		}

		#endregion
	}

	public class DumpVisitor : IGoldVisitor
	{
		private StringBuilder m_buffer;
		private int m_level;

		public DumpVisitor()
		{
			m_buffer = new StringBuilder();
		}

		#region IGoldVisitor Members

		public void Visit(Reduction p_reduction)
		{
			Print(p_reduction.ToString());
			m_level++;
			p_reduction.ChildrenAccept(this);
			m_level--;
		}

		#endregion

		public String GetResult()
		{
			return m_buffer.ToString();
		}

		private void Print(String p_string)
		{
			m_buffer.Append(new String(' ', m_level));
			m_buffer.Append(p_string).Append("\n");
		}
	}
}
