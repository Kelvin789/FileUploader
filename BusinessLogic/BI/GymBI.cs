using System;
using System.Collections.Generic;
using BusinessLogic.Helper;
using BusinessLogic.ViewModels;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using BusinessLogic.Models;
using System.Linq;

namespace BusinessLogic.BI
{
    public class GymBI
    {
        GymEntities db = new GymEntities();

        public UploaderViewResult BeginUploadProcess(BytesToFile File, bool IsSave)
        {
            UploaderViewResult IVR = new UploaderViewResult();
            IVR = ParseUploadedCSV(File);

            // Return if any errors found
            if (IVR.Validation != "")
            {
                return IVR;
            }

            IVR.RecordsCreated = IVR.RecordBuilderList.Count;

            foreach (RecordBuilder record in IVR.RecordBuilderList)
            {
                GymTracker GymTracker = BuildGymTracker(record);
                db.GymTrackers.Add(GymTracker);
            }

            // Return if any errors found
            if (IVR.Validation != "")
            {
                return IVR;
            }

            if (IsSave)
            {
                db.SaveChanges();
            }

            return IVR;
        }

        /// <summary>
        /// Validates file and trys to put content in DataTable
        /// </summary>
        /// <param name="File">File inputted</param>
        /// <returns></returns>
        public UploaderViewResult ParseUploadedCSV(BytesToFile File)
        {
            UploaderViewResult IVR = new UploaderViewResult
            {
                Validation = "",
                RecordBuilderList = new List<RecordBuilder>()
            };

            if (File._ContentLength > 0)
            {
                File._InputStream.Position = 0;
                TextFieldParser Parser = new TextFieldParser(File._InputStream);
                Parser.SetDelimiters(new string[] { "," });

                DataTable UploadInput = new DataTable("UploadInput");

                UploadInput.Columns.Add("Date", typeof(DateTime));
                UploadInput.Columns.Add("BodyPart", typeof(string)).DefaultValue = "";
                UploadInput.Columns.Add("Exercise", typeof(string)).DefaultValue = "";
                UploadInput.Columns.Add("Sets", typeof(string)).DefaultValue = "";
                UploadInput.Columns.Add("Reps", typeof(string)).DefaultValue = "";
                UploadInput.Columns.Add("Weight", typeof(string)).DefaultValue = "";

                string[] TempResults = Parser.ReadFields();
                TempResults = Parser.ReadFields(); // To skip headers and reads second line

                while (TempResults != null)
                {
                    DataRow Record = UploadInput.NewRow();
                    int Count = TempResults.GetUpperBound(0);

                    for (int i = 0; i <= Count; i++)
                    {
                        Type TargetType = Record[i].GetType();
                        Type SourceType = TempResults[i].GetType();
                        string ErrorColumn = UploadInput.Columns[i].ColumnName;

                        // Data type validation
                        if (i == 0 && (!DateTime.TryParse(TempResults[i].Trim(), out DateTime DateTimeConverted)))
                        {
                            IVR.Validation = "Invalid input within file: column " + ErrorColumn + " contains an invalid cell. "
                                             + TargetType.Name + " expected where " + SourceType.Name + ": '" + TempResults[i] + "' is provided.";
                            return IVR;
                        }
                        //else if ((i == 3 || i == 4) && (!int.TryParse(TempResults[i].Trim(), out int IntConverted)))
                        //{
                        //    IVR.Validation = "Invalid input within file: column " + ErrorColumn + " contains an invalid cell. "
                        //                     + TargetType.Name + " expected where " + SourceType.Name + ": '" + TempResults[i] + "' is provided.";
                        //    return IVR;
                        //}

                        Record[i] = TempResults[i].Trim();
                    }

                    UploadInput.Rows.Add(Record);
                    TempResults = Parser.ReadFields();
                }

                IVR.RecordBuilderList = BuildRecords(UploadInput);
            }

            return IVR;
        }

        /// <summary>
        /// Tries to return a list of RecordBuilder to later save
        /// </summary>
        /// <param name="UploadInput">Validated data</param>
        /// <returns></returns>
        public List<RecordBuilder> BuildRecords(DataTable UploadInput)
        {
            DateTime TempDate = new DateTime(0001, 1, 1);
            string TempBodyPart = "";
            string TempExercise = "";
            string TempSets = "";
            string TempReps = "";
            string TempWeight = "";

            string CurrentLine = "";
            string PreviousLine = "";
            int DataRowCount = UploadInput.Select().Length;
            int Count = 0;

            List<RecordBuilder> RecordBuildersList = new List<RecordBuilder>();
            RecordBuilder RecordBuilder = new RecordBuilder();

            foreach (var item in UploadInput.AsEnumerable())
            {
                Count++;
                TempDate = item.Field<DateTime>("Date");
                TempBodyPart = item.Field<string>("BodyPart");
                TempExercise = item.Field<string>("Exercise");
                TempSets = item.Field<string>("Sets"); 
                TempReps = item.Field<string>("Reps");
                TempWeight = item.Field<string>("Weight");

                CurrentLine = TempDate + TempBodyPart + TempExercise + TempSets + TempReps + TempWeight;

                if (Count != 1 && (CurrentLine != PreviousLine))
                {
                    RecordBuildersList.Add(RecordBuilder);
                    RecordBuilder = new RecordBuilder();
                }

                if (CurrentLine != PreviousLine)
                {
                    RecordBuilder = new RecordBuilder
                    {
                        DateCreated = TempDate,
                        BodyPart = TempBodyPart,
                        Exercise = TempExercise,
                        Sets = TempSets,
                        Reps = TempReps,
                        Weights = TempWeight
                    };
                }

                PreviousLine = TempDate + TempBodyPart + TempExercise + TempSets + TempReps + TempWeight;

                if (DataRowCount == Count)
                {
                    RecordBuildersList.Add(RecordBuilder);
                }
            }

            return RecordBuildersList;
        }

        public GymTracker BuildGymTracker(RecordBuilder RecordBuilder)
        {
            GymTracker GymTracker = new GymTracker
            {
                DateCreated = RecordBuilder.DateCreated,
                BodyPart = RecordBuilder.BodyPart,
                Exercise = RecordBuilder.Exercise,
                Sets = RecordBuilder.Sets,
                Reps = RecordBuilder.Reps,
                Weights = RecordBuilder.Weights
            };

            return GymTracker;
        }

        public List<GymTracker> GetAllGymTrackerRecords()
        {
            List<GymTracker> List = db.Set<GymTracker>().ToList();

            return List.ToList();
        }
    }
}
