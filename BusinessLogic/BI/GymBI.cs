using System;
using System.Collections.Generic;
using BusinessLogic.Helper;
using BusinessLogic.ViewModels;
using Microsoft.VisualBasic.FileIO;
using System.Data;
//using FileUploader.Models;

namespace BusinessLogic.BI
{
    public class GymBI
    {
        public ImportViewResult BeginImportProcess(BytesToFile File, bool IsSave)
        {
            ImportViewResult IVR = new ImportViewResult();
            IVR = ParseImportedCSV(File);

            if (IVR.Validation != "")
            {
                return IVR;
            }

            int CreatedRecordCount = 0;

            foreach (var item in IVR.RecordBuilderList)
            {
                // model = CreateRecord(IsSave)
            }

            if (IsSave == true)
            {
                //Save();
            }

            return IVR;
        }

        public ImportViewResult ParseImportedCSV(BytesToFile File)
        {
            ImportViewResult IVR = new ImportViewResult
            {
                Validation = "",
                RecordBuilderList = new List<RecordBuilder>()
            };

            if (File._ContentLength > 0)
            {
                File._InputStream.Position = 0;
                TextFieldParser Parser = new TextFieldParser(File._InputStream);
                Parser.SetDelimiters(new string[] { "," });

                DataTable ImportInput = new DataTable("ImportInput");

                ImportInput.Columns.Add("Date", typeof(DateTime)).DefaultValue = new DateTime(0001, 0, 0);
                ImportInput.Columns.Add("BodyPart", typeof(string)).DefaultValue = "";
                ImportInput.Columns.Add("Exercise", typeof(string)).DefaultValue = "";
                ImportInput.Columns.Add("Sets", typeof(int)).DefaultValue = 0;
                ImportInput.Columns.Add("Reps", typeof(int)).DefaultValue = 0;

                string[] TempResults = Parser.ReadFields();
                TempResults = Parser.ReadFields(); // To skip headers and reads second line

                while (TempResults != null)
                {
                    DataRow Record = ImportInput.NewRow();
                    int Count = TempResults.GetUpperBound(0);

                    for (int i = 0; i <= Count; i++)
                    {
                        Type TargetType = Record[i].GetType();
                        Type SourceType = TempResults[i].GetType();
                        string ErrorColumn = ImportInput.Columns[i].ColumnName;

                        if (i == 0 && (!DateTime.TryParse(TempResults[i].Trim(), out DateTime DateTimeConverted)))
                        {
                            IVR.Validation = "Invalid input within file: column " + ErrorColumn + " contains an invalid cell. "
                                             + TargetType.Name + " expected where " + SourceType.Name + ": '" + TempResults[i] + "' is provided.";
                            return IVR;
                        }
                        else if ((i == 3 || i == 4) && (!int.TryParse(TempResults[i].Trim(), out int IntConverted)))
                        {
                            IVR.Validation = "Invalid input within file: column " + ErrorColumn + " contains an invalid cell. "
                                             + TargetType.Name + " expected where " + SourceType.Name + ": '" + TempResults[i] + "' is provided.";
                            return IVR;
                        }

                        Record[i] = TempResults[i].Trim();
                    }

                    ImportInput.Rows.Add(Record);
                    TempResults = Parser.ReadFields();
                }

                IVR.RecordBuilderList = BuildRecords(ImportInput);
            }

            return IVR;
        }

        public List<RecordBuilder> BuildRecords(DataTable ImportInput)
        {
            DateTime TempDate = new DateTime(0001, 1, 1);
            string TempBodyPart = "";
            string TempExercise = "";
            int TempSets = 0;
            int TempReps = 0;

            string CurrentLine = "";
            string PreviousLine = "";
            int DataRowCount = ImportInput.Select().Length;
            int Count = 0;

            List<RecordBuilder> RecordBuildersList = new List<RecordBuilder>();
            RecordBuilder RecordBuilder = new RecordBuilder();

            foreach (var item in ImportInput.AsEnumerable())
            {
                Count++;
                TempDate = item.Field<DateTime>("Date");
                TempBodyPart = item.Field<string>("BodyPart");
                TempExercise = item.Field<string>("Exercise");
                TempSets = item.Field<int>("Sets"); 
                TempReps = item.Field<int>("Reps");

                CurrentLine = TempDate + TempBodyPart + TempExercise + TempSets + TempReps;

                if (Count != 1 && (CurrentLine != PreviousLine))
                {
                    RecordBuildersList.Add(RecordBuilder);
                    RecordBuilder = new RecordBuilder();
                }

                if (CurrentLine != PreviousLine)
                {
                    RecordBuilder = new RecordBuilder
                    {
                        DateTime = TempDate,
                        BodyPart = TempBodyPart,
                        Exercise = TempExercise,
                        Sets = TempSets,
                        Reps = TempReps
                    };
                }

                PreviousLine = TempDate + TempBodyPart + TempExercise + TempSets + TempReps;

                if (DataRowCount == Count)
                {
                    RecordBuildersList.Add(RecordBuilder);
                }
            }

            return RecordBuildersList;
        }

        public void CreateRecord(RecordBuilder RecordBuilder, bool IsSave)
        {
            // model ini

            //GymTracker GymTracker = new GymTracker();

            //foreach (var item in collection)
            //{

            //}
        }
    }
}
