using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.CodeDom.Compiler;
using System.Runtime.Remoting.Messaging;
using System.IO;
using System.CodeDom;

namespace NHibernate.Tool.hbm2net.T4
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    internal class PropertyProcessor:DirectiveProcessor
    {
        CodeDomProvider languageProvider;

        public override void Initialize(ITextTemplatingEngineHost host)
        {
            base.Initialize(host);
        }
        public override void StartProcessingRun(CodeDomProvider languageProvider, string templateContents, System.CodeDom.Compiler.CompilerErrorCollection errors)
        {
            callContextMembersWriter = new StringWriter();
            fieldWriter = new StringWriter();
            this.languageProvider = languageProvider;
            base.StartProcessingRun(languageProvider, templateContents, errors);
        }
        public override void FinishProcessingRun()
        {
            
        }
        StringWriter callContextMembersWriter;
        StringWriter fieldWriter;
        public override string GetClassCodeForProcessingRun()
        {
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            var langTool = new CodeMemberField()
            {
                Attributes = MemberAttributes.Public
                ,Name="languageTool"
                ,Type = new CodeTypeReference(typeof(LanguageTool))
                , InitExpression = new CodeObjectCreateExpression(typeof(LanguageTool))
            };
            languageProvider.GenerateCodeFromMember(langTool, fieldWriter, options);
            return fieldWriter.GetStringBuilder().ToString();
        }

        private void AssignMemberFromCallContext(string name,string type, TextWriter sw)
        {
            var fieldRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),name);
            var assign = new CodeAssignStatement(fieldRef,new CodeCastExpression(new CodeTypeReference(type),new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(CallContext))
                                                                ,"LogicalGetData"
                                                                ,new CodeExpression[]{new CodePrimitiveExpression(name)}
                                                                )));
            ;
            languageProvider.GenerateCodeFromStatement(assign, sw, new CodeGeneratorOptions());
        }

        public override string[] GetImportsForProcessingRun()
        {
            return new string[0];
        }

        public override string GetPostInitializationCodeForProcessingRun()
        {
            return callContextMembersWriter.GetStringBuilder().ToString();
        }

        public override string GetPreInitializationCodeForProcessingRun()
        {
            
            return "";
        }

        public override string[] GetReferencesForProcessingRun()
        {
            return new string[0];
        }

        public override bool IsDirectiveSupported(string directiveName)
        {
            return directiveName=="property";
        }

        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            AssignMemberFromCallContext(arguments["name"], arguments["type"], callContextMembersWriter);
            CreatePublicField(arguments["name"],arguments["type"],fieldWriter);
        }

        private void CreatePublicField(string name, string type, TextWriter writer)
        {
            CodeMemberField mf = new CodeMemberField()
            {
                Attributes = MemberAttributes.Public
                ,
                Name = name
                ,
                Type = new CodeTypeReference(type)
            };
            languageProvider.GenerateCodeFromMember(mf,writer,new CodeGeneratorOptions());
        }
    }
}
