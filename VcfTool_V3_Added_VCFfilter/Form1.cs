using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace VcfTool_V3_Added_VCFfilter
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog inputFolder;
        FolderBrowserDialog outputFolder;
        OpenFileDialog openFileDialog;
        //StringBuilder finalVcfFileData;
        //StringBuilder finalCsvFiledata;
        Thread Workerthread;

        StringBuilder Data2Save;
        string FileExtentionToUser;

        int threadTriger;

        StringBuilder ErrorFileNames = new StringBuilder();
        StringBuilder LogFile = new StringBuilder();

        string logFileName = DateTime.Now.ToString("dd-M-yyy-HH:mm:ss") + ".txt";

        enum threadExitStatus
        {
            beforeExecution,
            Success,
            Error

        }
        threadExitStatus threadStatus;

        public Form1()
        {
            InitializeComponent();

        }

        //Delegates Declare HERE

        delegate void updateLableDelagate(Label lbl, string text);
        delegate void updateTextBoxDelegate(TextBox txtbx, string text, Boolean append);
        delegate void enableControlDelegate(Control control, Boolean flag);

        delegate Boolean getCheckBoxValDelegate(CheckBox chbx);
        delegate String getTextBoxValDelegate(TextBox tbx);

        // Delegate Functions HERE

        private Boolean getCheckBoxValFunction(CheckBox ckbx) {

            return ckbx.Checked;

        }

        private String getTextBoxValFunctio(TextBox tbx) {

            return tbx.Text;
        }

        private void updateLableFunction(Label lbl, string txt)
        {
            lbl.Text = txt;

        }

        private void enableControlFunction(Control ctrl, Boolean flag)
        {
            ctrl.Enabled = flag;
        }

        private void updateTextBoxFunction(TextBox txtbx, string txt, Boolean append)
        {
            if (append)
            {
                txtbx.AppendText(txt);
                txtbx.Update();
            }
            else
            {
                txtbx.Text = txt;
                txtbx.Update();
            }
        }

        // ThreadSafe Functions HERE

        public void threadSafeLableUpdate(Label lbl, string txt)
        {
            if (lbl.InvokeRequired)
            {
                updateLableDelagate d = new updateLableDelagate(updateLableFunction);
                this.Invoke(d, new object[] { lbl, txt });
            }
            else
            {
                lbl.Text = txt;
                lbl.Update();
            }
        }

        public void threadSafeEnableControl(Control ctrl, Boolean flag)
        {
            if (ctrl.InvokeRequired)
            {
                enableControlDelegate d = new enableControlDelegate(enableControlFunction);
                this.Invoke(d, new object[] { ctrl, flag });
            }
            else
            {
                ctrl.Enabled = flag;
            }
        }
        public void threadSafeTextBoxUpdate(TextBox txtbx, string txt, Boolean append)
        {
            if (txtbx.InvokeRequired)
            {
                updateTextBoxDelegate d = new updateTextBoxDelegate(updateTextBoxFunction);
                this.Invoke(d, new object[] { txtbx, txt, append });
            }
            else
            {

                if (append)
                {
                    txtbx.AppendText(txt);
                    txtbx.Update();
                }
                else
                {
                    txtbx.Text = txt;
                    txtbx.Update();
                }
            }
        }

        public String threadSafeTextBoxRead(TextBox txtbx)
        {
            if (txtbx.InvokeRequired)
            {
                getTextBoxValDelegate d = new getTextBoxValDelegate(getTextBoxValFunctio);
                
                return (String) this.Invoke(d, new object[] { txtbx});
            }
            else
            {

                return txtbx.Text;
            }
        }

        public Boolean threadSafeCheckBoxRead(CheckBox ckbx)
        {
            if (ckbx.InvokeRequired)
            {
                getCheckBoxValDelegate d = new getCheckBoxValDelegate(getCheckBoxValFunction);

                return (Boolean) this.Invoke(d, new object[] {ckbx});
            }
            else
            {

                return ckbx.Checked;
            }
        }





        private void Form1_Load(object sender, EventArgs e)
        {
            lblSelectedTool.BackColor = Color.Red;
            lblSelectedTool.Text = "No Tool Selected";

            btnOpen.Enabled = false;
            btnGo.Enabled = false;

            lblFileOpen.BackColor = Color.Red;
            lblFileOpen.Text = "No File Selected";

            lbLinesCounter.Text = "";
            lblFileCounter.Text = "";

            ckBxFileMode.Checked = true;

            btnOpen.Text = "Select Folder";
        }

        private void cbxTools_DropDownClosed(object sender, EventArgs e)
        {
            lblSelectedTool.Text = cbxTools.SelectedItem.ToString();
            lblSelectedTool.BackColor = Color.Green;

            btnOpen.Enabled = true;
        }

        private void ckBxFileMode_CheckedChanged(object sender, EventArgs e)
        {
            if (ckBxFileMode.Checked) btnOpen.Text = "Select Folder";
            else btnOpen.Text = "Select File";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (ckBxFileMode.Checked)
            {
                inputFolder = new FolderBrowserDialog();
                inputFolder.ShowDialog();
                if (inputFolder.SelectedPath == "")
                {
                    MessageBox.Show("No Input Folder Was Selected!");
                    btnGo.Enabled = false;
                    lblFileOpen.BackColor = Color.Red;
                    lblFileOpen.Text = "No Folder Selected";
                }
                else
                {
                    btnGo.Enabled = true;
                    lblFileOpen.BackColor = Color.Green;
                    lblFileOpen.Text = inputFolder.SelectedPath;
                }

            }
        }

        private string RemoveUnwantedCharacters(string dirtyString)
        {

            string string2Return = "";
            dirtyString = dirtyString.Trim();

            if (dirtyString.Length > 0)
            {
                foreach (char letter in dirtyString)
                {
                    if (letter != ' ') string2Return = string2Return + letter.ToString();
                }

            }
            else string2Return = dirtyString;

            return string2Return;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (cbxTools.SelectedItem.ToString() == "Vcf To Csv")
            {
                FileExtentionToUser = ".csv";

                LogFile.AppendLine("Log File Created ON: " + DateTime.Now.ToString());
                LogFile.AppendLine("Log File Name: " + logFileName + FileExtentionToUser);
                LogFile.AppendLine("Error File: " + logFileName + " -Error File.txt");
                Workerthread = new Thread(new ThreadStart(Vcard2CSV));

                btnGo.Enabled = false;
                threadTriger = 0;
                Workerthread.Start();

            }

            else if (cbxTools.SelectedItem == "Apple Vcf to Nornal Vcf")
            {

                FileExtentionToUser = ".vcf";

                LogFile.AppendLine("Log File Created ON: " + DateTime.Now.ToString());
                LogFile.AppendLine("Log File Name: " + logFileName + FileExtentionToUser);
                LogFile.AppendLine("Error File: " + logFileName + " -Error File.txt");
                Workerthread = new Thread(new ThreadStart(iphone2NormalVcard));

                btnGo.Enabled = false;
                threadTriger = 0;
                Workerthread.Start();
            }


        }


        /// <summary>
        /// / File processing Methods Begin Herre.....
        /// </summary>
        private void iphone2NormalVcard()
        {
            try
            {
                string[] fileNames = new string[] { };
                if (ckBxFileMode.Checked)
                {
                    fileNames = Directory.GetFiles(inputFolder.SelectedPath, "*.vcf", SearchOption.AllDirectories);
                }

                if (fileNames.Length > 0)
                {
                    //outputFolder = new FolderBrowserDialog();
                    //outputFolder.SelectedPath=inputFolder.SelectedPath;
                    //outputFolder.ShowDialog();

                    long fileCount = 0;
                    Data2Save = new StringBuilder();

                    foreach (string fileName in fileNames)
                    {
                        fileCount++;

                        //tbxPreview.AppendText(fileName + "\n");
                        //lblFileCounter.Text = "Processing File: " + fileCount.ToString() + " of " + fileNames.Length.ToString();
                        //lblFileCounter.Update();

                        threadSafeTextBoxUpdate(tbxPreview, fileName + "\n", true);
                        threadSafeLableUpdate(lblFileCounter, "Processing File: " + fileCount.ToString() + " of " + fileNames.Length.ToString());

                        FileStream inputFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        StreamReader sReader = new StreamReader(inputFileStream);
                        string readLIne = sReader.ReadLine();

                        while (readLIne != null)
                        {

                            //lbLinesCounter.Text = "Now Processing Line: " + readLIne;
                            //lbLinesCounter.Update();

                            threadSafeLableUpdate(lbLinesCounter, "Now Processing Line: " + readLIne);
                            string LineWithoutSpaces = RemoveUnwantedCharacters(readLIne);

                            if (LineWithoutSpaces.Length > 1)
                            {
                                if (LineWithoutSpaces.Substring(0, 2) == "N:")
                                {
                                    Data2Save.AppendLine(LineWithoutSpaces);
                                }
                            }

                            if (LineWithoutSpaces.Length > 2)
                            {
                                if (LineWithoutSpaces.Substring(0, 3).ToUpper() == "FN:")
                                {
                                    Data2Save.AppendLine(LineWithoutSpaces);
                                }
                            }


                            if (LineWithoutSpaces.Length > 7)
                            {
                                if (LineWithoutSpaces.Substring(0, 8).ToUpper() == "VERSION:")
                                {
                                    Data2Save.AppendLine("VERSION:2.1");
                                }
                            }


                            if (LineWithoutSpaces.Length > 5)
                            {
                                if (LineWithoutSpaces.Substring(0, 6).ToUpper() == "BEGIN:")
                                {
                                    Data2Save.AppendLine(LineWithoutSpaces);
                                }
                            }

                            if (LineWithoutSpaces.Length > 3)
                            {
                                if (LineWithoutSpaces.Substring(0, 4).ToUpper() == "TEL;")
                                {
                                    string[] splitLine = LineWithoutSpaces.Split(new char[] { ':' });
                                    if (splitLine.Length != 2) Data2Save.AppendLine("TEL;CELL:0000000000");
                                    else
                                    {
                                        Data2Save.AppendLine("TEL;CELL:" + splitLine[1]);
                                    }
                                }
                            }

                            if (LineWithoutSpaces.Length > 5)
                            {
                                if (LineWithoutSpaces.Substring(0, 6).ToUpper() == "EMAIL;")
                                {
                                    string[] splitLine = LineWithoutSpaces.Split(new char[] { ':' });
                                    if (splitLine.Length != 2) Data2Save.AppendLine("EMAIL;WORK: ");
                                    else
                                    {
                                        Data2Save.AppendLine("EMAIL;WORK:" + splitLine[1]);
                                    }
                                }
                            }


                            if (LineWithoutSpaces.Length > 3)
                            {
                                if (LineWithoutSpaces.Substring(0, 4).ToUpper() == "ORG:")
                                {
                                    string[] splitLine = LineWithoutSpaces.Split(new char[] { ':' });
                                    if (splitLine.Length != 2) Data2Save.AppendLine("ORG: ");
                                    else
                                    {
                                        Data2Save.AppendLine("ORG:" + splitLine[1]);
                                    }
                                }
                            }

                            if (LineWithoutSpaces.Length > 5)
                            {
                                if (LineWithoutSpaces.Substring(0, 6).ToUpper() == "TITLE:")
                                {
                                    string[] splitLine = LineWithoutSpaces.Split(new char[] { ':' });
                                    if (splitLine.Length != 2) Data2Save.AppendLine("TITLE: ");
                                    else
                                    {
                                        Data2Save.AppendLine("TITLE:" + splitLine[1]);
                                    }
                                }
                            }

                            if (LineWithoutSpaces.Length > 3)
                            {
                                if (LineWithoutSpaces.Substring(0, 4).ToUpper() == "END:")
                                {
                                    Data2Save.AppendLine(LineWithoutSpaces);

                                }
                            }

                            readLIne = sReader.ReadLine();
                        }

                        inputFileStream.Close();

                    }



                }

                threadTriger = 1;
                threadStatus = threadExitStatus.Success;
                threadSafeEnableControl(btnGo, true);
            }
            catch (Exception ex)
            {
                threadStatus = threadExitStatus.Error;
                MessageBox.Show("File Parse Error: " + ex.Message);

                threadTriger = 1;
                threadSafeEnableControl(btnGo, true);
            }
        }

        private void Vcard2CSV()
        {

            try
            {

                string[] fileNames = new string[] { };
                if (ckBxFileMode.Checked)
                {
                    fileNames = Directory.GetFiles(inputFolder.SelectedPath, "*.vcf", SearchOption.AllDirectories);
                }

                if (fileNames.Length > 0)
                {
                    long fileCount = 0;
                    Data2Save = new StringBuilder();
                    Data2Save.AppendLine("\"FirstName\"," + "\"LastName\"," + "\"MiddleName\"," + "\"Phone1\"," + "\"Phone2\"," + "\"Phone3\"," + "\"Phone4\"," + "\"Phone5\"," + "\"Phone6\"," + "\"Email1\"," + "\"Email2\",");

                    foreach (string fileName in fileNames)
                    {
                        fileCount++;
                        LogFile.AppendLine(DateTime.Now.ToString() + " = Now Procesing: " + fileName);

                        threadSafeTextBoxUpdate(tbxPreview, fileName + "\n", true);
                        threadSafeLableUpdate(lblFileCounter, "Processing File: " + fileCount.ToString() + " of " + fileNames.Length.ToString());

                        Boolean ckBxVal = threadSafeCheckBoxRead(ckbxFilter);
                        String tbxVal = threadSafeTextBoxRead(tbxFilter);

                        VcardProcesssor VcardObj;

                        if (! ckBxVal) VcardObj = new VcardProcesssor(fileName, ErrorFileNames);
                        else {
                            if (tbxVal == "")
                            {

                              //  tbxFilter.BackColor = Color.Red;
                                throw new Exception("Please Provide Filter -ve Filter Text");
                            }
                            else {
                              //  tbxFilter.BackColor = Color.White;
                                VcardObj = new VcardProcesssor(fileName, ErrorFileNames, tbxFilter.Text);
                            }
                        }

                        
                        Data2Save = Data2Save.Append(VcardObj.GenerateCSVFile(false));
                    }

                }

                threadTriger = 1;
                threadStatus = threadExitStatus.Success;
                threadSafeEnableControl(btnGo, true);


            }
            catch (Exception ex)
            {

                threadStatus = threadExitStatus.Error;
                MessageBox.Show("File Parse Error: " + ex.Message.ToString(), "Vcard2Csv");

                threadTriger = 1;
                threadSafeEnableControl(btnGo, true);
            }

        }


        private void btnGo_EnabledChanged(object sender, EventArgs e)
        {

            if (threadTriger == 1)
            {
                if (threadStatus == threadExitStatus.Error)
                {
                    MessageBox.Show("Thread Existed With Error");
                }
                else if (threadStatus == threadExitStatus.Success)
                {

                    TextWriter txtWriter;

                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.ShowDialog();
                    if (saveFile.FileName == "")
                    {
                        txtWriter = File.CreateText(inputFolder.SelectedPath + "\\FinalMergedVcard.csv");
                        txtWriter.Write(Data2Save.ToString());
                        txtWriter.Close();

                        TextWriter txtWriterLogFile = File.CreateText(inputFolder.SelectedPath + "\\" + logFileName + ".txt");
                        txtWriterLogFile.Write(LogFile.ToString());
                        txtWriterLogFile.Close();

                        TextWriter txtWriterErrorFile = File.CreateText(inputFolder.SelectedPath + "\\" + logFileName + " -Error File.txt");
                        txtWriterErrorFile.Write(LogFile.ToString());
                        txtWriterErrorFile.Close();
                    }
                    else
                    {
                        txtWriter = File.CreateText(saveFile.FileName);
                        txtWriter.Write(Data2Save.ToString());
                        txtWriter.Close();

                        TextWriter txtWriterLogFile = File.CreateText(Path.GetDirectoryName(saveFile.FileName) + "\\" + logFileName + ".txt");
                        txtWriterLogFile.Write(LogFile.ToString());
                        txtWriterLogFile.Close();

                        TextWriter txtWriterErrorFile = File.CreateText(Path.GetDirectoryName(saveFile.FileName) + "\\" + logFileName + " -Error File.txt");
                        txtWriterErrorFile.Write(ErrorFileNames.ToString());
                        txtWriterErrorFile.Close();
                    }

                    MessageBox.Show("Done.....");

                }
            }

        }

        private void ckbxFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxFilter.Checked) tbxFilter.Enabled = true;
            else tbxFilter.Enabled = false;
        }
    }
}
