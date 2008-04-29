using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI;

namespace NHibernate.Burrow.WebUtil.Impl
{
    internal class StatefulFieldPageModule
    {
        private readonly GlobalPlaceHolder gph;
        private readonly Page page;
        protected bool dataLoaded = false;
    	private IDictionary<string, StateBag> states = new Dictionary<string,StateBag>();
    	private const string Seperator = "_NHB_";
    	private const string Prefix = "NHibernate.Burrow.WebUtil.StatefulField";

		public IDictionary<string, StateBag> States
		{
			get { return states; }
    	}

    	public GlobalPlaceHolder GlobalPlaceHolder {
			get { return gph; }
    	}


        public StatefulFieldPageModule(Page page, GlobalPlaceHolder globalPlaceHolder)
        {
            this.page = page;
            gph = globalPlaceHolder;
			
            page.PreLoad += new EventHandler(LoadData);
            page.PreRenderComplete += new EventHandler(page_PreRenderComplete);
        	
        }


		/// <summary>
		/// process has to happen after all controls are initiated otherwise will cause ASP.net control initiation problem - sub control even failed to register
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadData(object sender, EventArgs e)
		{
			if (!page.IsPostBack || dataLoaded)
			{
				return;
			}
			dataLoaded = true;
			LoadStates();
			new StatefulFieldLoader(page, this).Process();
		}


    	private void page_PreRenderComplete(object sender, EventArgs e)
        {
            new StatefulFieldSaver(page, this).Process();
			SaveStates();
        }


		private void LoadStates()
		{
			NameValueCollection form = page.Request.Form;
			foreach (string key in form.AllKeys)
			{ 
				if(key == null) return;
				string[] keys = key.Split(new string[] { Seperator }, StringSplitOptions.RemoveEmptyEntries);
				if (keys.Length == 3 && keys[0] == Prefix)
				{
					GetControlState(keys[1]).Add(keys[2], Deserialize(form[key]));
				}
			}
		}

		public StateBag GetControlState(string controlUID)
		{
			StateBag retVal;
			if(! states.TryGetValue(controlUID, out retVal))
				states[controlUID] = retVal = new StateBag();
			return retVal;
		}


		private void SaveStates()
		{
			foreach (KeyValuePair<string, StateBag> pair in states) {
				if (pair.Value != null) {
					string stateKeyPrefix = Prefix + Seperator + pair.Key + Seperator; 
					foreach (DictionaryEntry state in pair.Value) {
						string value = Serialize(state.Value != null ? ((StateItem) state.Value).Value : null);
						GlobalPlaceHolder.AddPostBackField(stateKeyPrefix + (string) state.Key, value);
					}
				}
			}
		}

    	 

    	private object Deserialize(string value)
		{
			LosFormatter lf = new LosFormatter();
			return lf.Deserialize(value);
		}

		private string Serialize(object val)
		{
			LosFormatter lf = new LosFormatter();
			TextWriter tw = new StringWriter();
			lf.Serialize(tw, val);
			return tw.ToString();
		}

	
    }
}