﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;
using ATPTennisStat.Importers.Contracts;
using ATPTennisStat.SQLServerData;
using ATPTennisStat.Models;
using ATPTennisStat.Factories;

namespace ATPTennisStat.Importers
{
    public class ExcelImporter : IImporter
    {
        private SqlServerDataProvider dataProvider;
        private ModelsFactory modelsFactory;

        public ExcelImporter(SqlServerDataProvider dataProvider, ModelsFactory modelsFactory)
        {
            this.dataProvider = dataProvider;
            this.modelsFactory = modelsFactory;

        }

        public void Read()
        {
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

            string path = dir + "\\Data\\Excel\\TennisStatsDatabase.xlsx";
            Console.WriteLine(path);
            var workbook = new XLWorkbook(path);
            var ws = workbook.Worksheet(1);
            Console.WriteLine(ws.Name);
            var currentRegion = ws.RangeUsed().AsTable();
            var names = currentRegion.DataRange.Rows()
                .Select(nameRow => nameRow.Field("Name").GetString())
                .ToList();
            names.ForEach(Console.WriteLine);
        }

        public void Write()
        {

            //ISsue when adding two different cities in batch
            //Trackign changes - attached status?
            var city = this.modelsFactory.CreateCity("Paris", "France");
            this.dataProvider.Cities.Add(city);

            //this.dataProvider.UnitOfWork.Finished();

            var city1 = modelsFactory.CreateCity("Nant", "France");


            this.dataProvider.Cities.Add(city1);

            //this.dataProvider.UnitOfWork.Finished();

        }


    }
}
