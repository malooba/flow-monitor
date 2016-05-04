//Copyright 2016 Malooba Ltd

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ScintillaNET;

namespace Diagram.DiagramModel.PropertyEditors
{
    public sealed partial class TextEditor : Form
    {
        private DataType datatype;

        public JToken Result;

        public TextEditor(string type, JToken value)
        {
            InitializeComponent();

            datatype = DataType.FromString(type);
            switch(type)
            {
                case "json":
                    SetJson();
                    Text = "JSON editor";
                    break;

                case "javascript":
                    SetJavaScript();
                    Text = "JavaScript editor";
                    break;

                case "xml":
                case "xmlfragment":
                    SetXml();
                    Text = "XML editor";
                    break;

                case "string":
                    SetText();
                    Text = "Text editor";
                    break;

                default:
                    SetText();
                    Text = "Data editor";
                    break;
            }
            editor.Text = datatype.GetEditorString(value);
        }

        private int currentLineNumberCharLength;
        private void scintillaTextChanged(object sender, EventArgs e)
        {
            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = editor.Lines.Count.ToString().Length;
            if(maxLineNumberCharLength == currentLineNumberCharLength)
                return;

            editor.Margins[0].Width = editor.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + 2;
            currentLineNumberCharLength = maxLineNumberCharLength;
        }

        void SetJavaScript()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            editor.StyleResetDefault();
            editor.Styles[Style.Default].Font = "Consolas";
            editor.Styles[Style.Default].Size = 10;
            editor.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            editor.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            editor.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            editor.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            editor.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            editor.Styles[Style.Cpp.Number].ForeColor = Color.YellowGreen;
            editor.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            editor.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            editor.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Cpp.StringEol].BackColor = Color.Purple;
            editor.Styles[Style.Cpp.Operator].ForeColor = Color.MidnightBlue;
            editor.Lexer = Lexer.Cpp;
            editor.SetKeywords(0, "abstract boolean break byte case catch char class const continue " +
                                  "debugger default delete do double else enum export extends final " +
                                  "finally float for function goto if implements import in instanceof " +
                                  "int interface long native new package private protected public return " +
                                  "short static super switch synchronized this throw throws transient " +
                                  "try typeof var void volatile while with");
        }

        void SetJson()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            editor.StyleResetDefault();
            editor.Styles[Style.Default].Font = "Consolas";
            editor.Styles[Style.Default].Size = 10;
            editor.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            editor.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            editor.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            editor.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            editor.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            editor.Styles[Style.Cpp.Number].ForeColor = Color.YellowGreen;
            editor.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            editor.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            editor.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Cpp.StringEol].BackColor = Color.Purple;
            editor.Styles[Style.Cpp.Operator].ForeColor = Color.DodgerBlue;
            editor.Lexer = Lexer.Cpp;
        }

        void SetXml()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            editor.StyleResetDefault();
            editor.Styles[Style.Default].Font = "Consolas";
            editor.Styles[Style.Default].Size = 10;
            editor.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            editor.Styles[Style.Xml.Default].ForeColor = Color.Silver;
            editor.Styles[Style.Xml.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            editor.Styles[Style.Xml.Number].ForeColor = Color.YellowGreen;
            editor.Styles[Style.Xml.SingleString].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Xml.DoubleString].ForeColor = Color.FromArgb(163, 21, 21); // Red
            editor.Styles[Style.Xml.Entity].ForeColor = Color.MidnightBlue;
            editor.Lexer = Lexer.Xml;
        }

        void SetText()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            editor.StyleResetDefault();
            editor.Styles[Style.Default].Font = "Consolas";
            editor.Styles[Style.Default].Size = 10;
            editor.StyleClearAll();
            editor.Lexer = Lexer.Null;
        }

        private void OnOk(object sender, EventArgs e)
        {
            try
            {
                Result = datatype.Parse(editor.Text);
                DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show(this, "Invalid input for datatype", "Input error");
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {

        }

        private void EditorCut(object sender, EventArgs e)
        {
            editor.Cut();
        }

        private void EditorCopy(object sender, EventArgs e)
        {
            editor.Copy();
        }

        private void EditorPaste(object sender, EventArgs e)
        {
            editor.Paste();
        }
    }
}