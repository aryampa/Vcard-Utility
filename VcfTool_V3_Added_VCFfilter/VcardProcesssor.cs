using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MyProject.vCard;

namespace VcfTool_V3_Added_VCFfilter
{
    class VcardProcesssor
    {
        public long TotalContacts;
        public List<string> listOfRawContactStrings;
        FileStream fs;

        FileStream fsError;
        StreamWriter sWriter;

        struct contactStrcture
        {
            //FullName = 0;
            // SurName = 1;
            //GivenName = 2;
            //MiddleName = 3;
            public string FullName;
            public List<string> Names;
            public string Company;
            public List<string> Phones;
            public List<string> Emails;


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

        private contactStrcture buildContacts(string TextWithConatcts)
        {
            contactStrcture contactItem = new contactStrcture();

            contactItem.Company = "";
            contactItem.Emails = new List<string> { };
            contactItem.Phones = new List<string> { };
            contactItem.Names = new List<string> { };
            contactItem.FullName = "";

            string[] arrayOfLines = TextWithConatcts.Split(new char[] { '\n' });

            if (arrayOfLines.Length > 2)
            {

                foreach (string line in arrayOfLines)
                {



                    //this.threadSafeLableUpdate(this.lbLinesCounter, "Now Processing Line: " + readLIne);

                    string LineWithoutSpaces = RemoveUnwantedCharacters(line);


                    if (LineWithoutSpaces.Length > 2)
                    {
                        if (LineWithoutSpaces.Substring(0, 3).ToUpper() == "FN:")
                        {
                            string[] splitName = line.Split(new char[] { ':' });
                            if (splitName.Length > 1)
                            {
                                contactItem.FullName = splitName[1];
                                contactItem.Names.Add(splitName[1]);
                            }

                        }
                    }




                    if (LineWithoutSpaces.Length > 3)
                    {
                        if (LineWithoutSpaces.Substring(0, 4).ToUpper() == "TEL;")
                        {
                            string[] splitLine = LineWithoutSpaces.Split(new char[] { ':' });
                            if (splitLine.Length == 2)
                            {
                                contactItem.Phones.Add(splitLine[1]);
                            }

                        }
                    }

                    if (LineWithoutSpaces.Length > 5)
                    {
                        if (LineWithoutSpaces.Substring(0, 6).ToUpper() == "EMAIL;")
                        {
                            string[] splitLine = LineWithoutSpaces.Split(new char[] { ':' });
                            if (splitLine.Length == 2) contactItem.Emails.Add(splitLine[1]);

                        }
                    }


                    if (LineWithoutSpaces.Length > 3)
                    {
                        if (LineWithoutSpaces.Substring(0, 4).ToUpper() == "ORG:")
                        {
                            string[] splitLine = LineWithoutSpaces.Split(new char[] { ':' });
                            if (splitLine.Length == 2) contactItem.Company = splitLine[1];
                        }
                    }


                }

            }


            return contactItem;

        }

        public StringBuilder GenerateCSVFile(Boolean appendHeader)
        {
            StringBuilder sb2Return = new StringBuilder();

            if (appendHeader) sb2Return.AppendLine("\"FirstName\"," + "\"LastName\"," + "\"MiddleName\"," + "\"Phone1\"," + "\"Phone2\"," + "\"Phone3\"," + "\"Phone4\"," + "\"Phone5\"," + "\"Phone6\"," + "\"Email1\"," + "\"Email2\",");


            foreach (string contactString in listOfRawContactStrings)
            {

                contactStrcture newContactItem = buildContacts(contactString);
                if (newContactItem.Names.Count == 0 | newContactItem.Phones.Count == 0)
                {
                }
                else
                {


                    string contactRowString = "";

                    for (int i = 0; i < 3; i++)
                    {
                        //errorString = newContactItem.Names.Count.ToString();
                        //errorString2 = i.ToString();
                        string string2append = "";
                        if (i < newContactItem.Names.Count) string2append = newContactItem.Names[i].ToString();
                        contactRowString = contactRowString + "\"" + string2append + "\",";
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        string string2append = "";
                        if (i < newContactItem.Phones.Count) string2append = newContactItem.Phones[i].ToString();
                        contactRowString = contactRowString + "\"" + string2append + "\",";
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        string string2append = "";
                        if (i < newContactItem.Emails.Count) string2append = newContactItem.Emails[i].ToString();
                        contactRowString = contactRowString + "\"" + string2append + "\",";
                    }

                    sb2Return.AppendLine(contactRowString);
                }

            }

            return sb2Return;

        }

        public VcardProcesssor(string InputFileName, StringBuilder ErrorFileNames, String FilterLine)
        {
            Boolean Contact_OK = true;

            listOfRawContactStrings = new List<string> { };
            if (InputFileName != "")
            {

                try
                {
                    //if (LogFileStream  == null) throw new Exception("No Output Dirrectory was provided");
                    //else {

                    //    //fsError = new FileStream(ErrorOutputDir+"ErrorFile.txt          ", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    //    //sWriter = new StreamWriter(LogFileStream );
                    //    //sWriter.WriteLine(">>>>>>>>>  Error File Created ON: " + DateTime.Now.ToString() +"     >>>>>>>>>>>>>>>>>>>");
                    //}
                    string fileExt = Path.GetExtension(InputFileName).ToLower();

                    if (fileExt != ".vcf") throw new Exception("Selected File Is Not a VCard!");

                    fs = new FileStream(InputFileName, FileMode.Open, FileAccess.Read);
                    StreamReader sReader = new StreamReader(fs);
                    Boolean beginSeen = false;
                    Boolean versionSeen = false;
                    Boolean endSeen = false;

                    string readLine = sReader.ReadLine();
                    if (readLine != "BEGIN:VCARD") throw new System.Exception("VCARD File Improperly formated!!!-No Begin Found");

                    int countConts = 0;
                    StringBuilder sBuilder = new StringBuilder();
                    StringBuilder sBuilderAllContacts = new StringBuilder();
                    while (readLine != null)
                    {
                        if (readLine == "BEGIN:VCARD")
                        {

                            if (beginSeen) throw new System.Exception("VCARD File Improperly formated!!! - Begin seen twice: " + readLine);
                            else
                            {
                                beginSeen = true;
                                endSeen = false;

                                Contact_OK = true;

                                sBuilder.AppendLine(readLine);

                            }

                        }





                        else if ((readLine == "VERSION:2.1" | readLine == "VERSION:3.0"))
                        {

                            if ((beginSeen == false | endSeen == true)) throw new System.Exception("VCARD File Improperly formated!!!--End seen before Begin (version)");
                            else { versionSeen = true; sBuilder.AppendLine(readLine); }
                        }


                        else if (readLine.ToLower().Trim() == FilterLine.ToLower().Trim())
                        {
                            Contact_OK = false;
                        }





                        else if (readLine == "END:VCARD")
                        {
                            if ((beginSeen == false | versionSeen == false)) throw new System.Exception("VCARD File Improperly formated!!! - End seen no begin no version");

                            else
                            {
                                beginSeen = false;
                                versionSeen = false;
                                endSeen = true;

                                if (Contact_OK) {

                                    listOfRawContactStrings.Add(sBuilder.ToString());
                                    countConts++;

                                }

                                sBuilderAllContacts.AppendLine(sBuilder.ToString());
                                
                                sBuilder.Length = 0;


                                sBuilderAllContacts.AppendLine("XXXX END OF CONTACT - " + countConts.ToString() + " XXXXXX");
                                sBuilderAllContacts.AppendLine();



                            }
                        }


                        else
                        {
                            sBuilder.AppendLine(readLine);
                        }

                        // sb_logText.AppendLine("Line : [" + readLine + "] Successfully processed!!!!");

                        readLine = sReader.ReadLine();
                    }

                    // textBox1.Text = sBuilderAllContacts.ToString(); ;

                    if (listOfRawContactStrings.Count == 0) throw new Exception("No valid Contact Data was Found in vcard");
                    else TotalContacts = listOfRawContactStrings.Count;

                    fs.Close();
                }

                catch (System.Exception ex)
                {
                    if (fs != null) fs.Close();
                    MessageBox.Show("Error Parsing VCARD: " + ex.Message.ToString());
                    ErrorFileNames.AppendLine(InputFileName);


                }


            }
        }

        public VcardProcesssor(string InputFileName, StringBuilder ErrorFileNames)
        {
            

            listOfRawContactStrings = new List<string> { };
            if (InputFileName != "")
            {

                try
                {
                    //if (LogFileStream  == null) throw new Exception("No Output Dirrectory was provided");
                    //else {

                    //    //fsError = new FileStream(ErrorOutputDir+"ErrorFile.txt          ", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    //    //sWriter = new StreamWriter(LogFileStream );
                    //    //sWriter.WriteLine(">>>>>>>>>  Error File Created ON: " + DateTime.Now.ToString() +"     >>>>>>>>>>>>>>>>>>>");
                    //}
                    string fileExt = Path.GetExtension(InputFileName).ToLower();

                    if (fileExt != ".vcf") throw new Exception("Selected File Is Not a VCard!");

                    fs = new FileStream(InputFileName, FileMode.Open, FileAccess.Read);
                    StreamReader sReader = new StreamReader(fs);
                    Boolean beginSeen = false;
                    Boolean versionSeen = false;
                    Boolean endSeen = false;

                    string readLine = sReader.ReadLine();
                    if (readLine != "BEGIN:VCARD") throw new System.Exception("VCARD File Improperly formated!!!-No Begin Found");

                    int countConts = 0;
                    StringBuilder sBuilder = new StringBuilder();
                    StringBuilder sBuilderAllContacts = new StringBuilder();
                    while (readLine != null)
                    {
                        if (readLine == "BEGIN:VCARD")
                        {

                            if (beginSeen) throw new System.Exception("VCARD File Improperly formated!!! - Begin seen twice: " + readLine);
                            else
                            {
                                beginSeen = true;
                                endSeen = false;

                                

                                sBuilder.AppendLine(readLine);

                            }

                        }





                        else if ((readLine == "VERSION:2.1" | readLine == "VERSION:3.0"))
                        {

                            if ((beginSeen == false | endSeen == true)) throw new System.Exception("VCARD File Improperly formated!!!--End seen before Begin (version)");
                            else { versionSeen = true; sBuilder.AppendLine(readLine); }
                        }






                        else if (readLine == "END:VCARD")
                        {
                            if ((beginSeen == false | versionSeen == false)) throw new System.Exception("VCARD File Improperly formated!!! - End seen no begin no version");

                            else
                            {
                                beginSeen = false;
                                versionSeen = false;
                                endSeen = true;

                                

                               listOfRawContactStrings.Add(sBuilder.ToString());
                               countConts++;

                                

                                sBuilderAllContacts.AppendLine(sBuilder.ToString());

                                sBuilder.Length = 0;


                                sBuilderAllContacts.AppendLine("XXXX END OF CONTACT - " + countConts.ToString() + " XXXXXX");
                                sBuilderAllContacts.AppendLine();



                            }
                        }


                        else
                        {
                            sBuilder.AppendLine(readLine);
                        }

                        // sb_logText.AppendLine("Line : [" + readLine + "] Successfully processed!!!!");

                        readLine = sReader.ReadLine();
                    }

                    // textBox1.Text = sBuilderAllContacts.ToString(); ;

                    if (listOfRawContactStrings.Count == 0) throw new Exception("No valid Contact Data was Found in vcard");
                    else TotalContacts = listOfRawContactStrings.Count;

                    fs.Close();
                }

                catch (System.Exception ex)
                {
                    if (fs != null) fs.Close();
                    MessageBox.Show("Error Parsing VCARD: " + ex.Message.ToString());
                    ErrorFileNames.AppendLine(InputFileName);


                }


            }
        }
    }
}
