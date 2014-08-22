using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public class ChatExtendedFunctions
    {
        private List<string> myOldMessages;
        private int myOldMessageIndex = -1;
        private int maxCapacity;

        public ChatExtendedFunctions()
        {
            myOldMessages = new List<string>();
            this.maxCapacity = 10;
        }
        public ChatExtendedFunctions(int maxCapacity)
        {
            myOldMessages = new List<string>();
            this.maxCapacity = maxCapacity;
        }

        public void AddMyNewMessage(string message)
        {
            myOldMessages.Add(message);

            if (myOldMessages.Count > maxCapacity)
            {
                myOldMessages.RemoveAt(0);
            }

            myOldMessageIndex = myOldMessages.Count;
        }
        public string GetNextOlderMessage()
        {
            if (myOldMessageIndex >= 1)
                myOldMessageIndex--;

            return myOldMessages[myOldMessageIndex];
        }
        public string GetNextYoungerMessage()
        {
            if (myOldMessageIndex <= (myOldMessages.Count - 2))
                myOldMessageIndex++;

            if (myOldMessageIndex == myOldMessages.Count)
            {
                return string.Empty;
            }

            return myOldMessages[myOldMessageIndex]; 
        }
        public void AddTextToChat(ref RichTextBox rTxtBox, string txt, Color col)
        {
            int pos = rTxtBox.TextLength;
            rTxtBox.AppendText(txt);
            rTxtBox.Select(pos, txt.Length);
            rTxtBox.SelectionColor = col;
            rTxtBox.Select();
        }
        public void AddTextToChat(ref RichTextBox rTxtBox, string txt)
        {
            AddTextToChat(ref rTxtBox, txt, rTxtBox.ForeColor);
        }
    }
}