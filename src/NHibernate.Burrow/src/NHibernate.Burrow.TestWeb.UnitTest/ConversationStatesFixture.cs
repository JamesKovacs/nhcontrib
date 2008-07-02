#region WatiN Copyright (C) 2006-2007 Jeroen van Menen

//Copyright 2006-2007 Jeroen van Menen
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

#endregion Copyright

using NUnit.Framework;
using WatiN.Core;
using WatiN.Core.Interfaces;
using WatiN.Core.UnitTests;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
 
    [TestFixture]
    public class ConversationStatesFixture : TestBase
    {
        private string page = "ConversationStates/ConversationNormal.aspx";

        [Test]
        public void CommitInBusinessTransactionTest()
        {
            CheckConversationMode("rbBusniess");
        } 
        
        [Test]
        public void CommitInLongDBTransactionTest()
        {
            CheckConversationMode("rbLongDB");
        } 
        
        [Test]
        public void CommitInNonAtmoicTest()
        {
            CheckConversationMode("rbNonAtmoic");
        }

        private void CheckConversationMode(string radioButtonName) {
            GoTo(page);
            IE.RadioButton("ConversationStates1_" + radioButtonName).Checked = true;
            IE.Link("ConversationStates1_btnStart").Click();
            AssertText("MockEntity In Conversation: 0");
            IE.Button("ConversationStates1_btnUpdate").Click();
            AssertText("MockEntity In Conversation: 1");
            IE.Button("ConversationStates1_btnCommit").Click();
            AssertText("Congratulations test passed! ");
            AssertText("MockEntity In Conversation: NULL");
            AssertText("MockEntity in DB: 1");
        }

        [Test]
        public void CancelTest()
        {
            GoTo(page);
            IE.Link("ConversationStates1_btnStart").Click();
            AssertText("MockEntity In Conversation: 0");
            AssertText("MockEntity in DB: NULL");
            IE.Button("ConversationStates1_btnUpdate").Click();
            AssertText("MockEntity In Conversation: 1");
            AssertText("MockEntity in DB: NULL");
            IE.Button("ConversationStates1_btnCancel").Click();
            AssertText("MockEntity In Conversation: NULL");
            AssertText("MockEntity in DB: NULL");
        }


        [Test]
        public void LazyLoadInConversationTest(){
            GoTo("ConversationStates/ConversationLazyLoad.aspx");
            IE.Button("btnNext").Click();
            AssertTestSuccessMessageShown();
        }
    }
}