#region Copyright
//  Copyright 2015 OSIsoft, LLC
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
#endregion
using System;
using System.ComponentModel;
using CommandLine;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.PI;
using Clues.Library;
using Clues.Library.DataHandling;

namespace Clues
{

    [Description("Utility to Create, Edit, Delete event frames from a csv file.")]
    [AdditionalDescription("You can run commands to test if the data parsing is correct.  To make the write occur against the server, you need to specify the --EnableWrite option.")]
    [UsageExample("AFEventFramesUtil -s SRV -d afDatabase -c -f data.csv")]
    public class AFEventFramesUtil : AppletBase
    {
        //Command line Options
        [Option('s', "AFServer", HelpText = "AF Server to connect to", Required = false)]
        public string Server { get; set; }

        [Option('d', "database", HelpText = "Name of the AF database use", Required = false)]
        public string Database { get; set; }

        [Option('c', "create", HelpText = "Create new event frames from the csv file passed", Required = false, MutuallyExclusiveSet = "CreateEditDeleteTest")]
        public bool Create { get; set; }

        [Option('e', "edit", HelpText = "Edit event frames", Required = false, MutuallyExclusiveSet = "CreateEditDeleteTest")]
        public bool Edit { get; set; }

        [Option('d', "delete", HelpText = "Name of the AF database use", Required = false, MutuallyExclusiveSet = "CreateEditDeleteTest")]
        public bool Delete { get; set; }

        [Option('t', "test", HelpText = "Name of the AF database use", Required = false, MutuallyExclusiveSet = "CreateEditDeleteTest")]
        public bool Test { get; set; }

        [Option('f', "file", HelpText = "Name of the AF database use", Required = true)]
        public string File { get; set; }

        [Option("EnableWrite", HelpText = "To enable the execution of write commands on the AF Server.", Required = false)]
        public bool EnableWrite { get; set; }

        private void CheckOption()
        {
            if ((Create || Edit || Delete) && EnableWrite)
            {
                if (string.IsNullOrEmpty(Database) || string.IsNullOrEmpty(Server))
                {
                    throw new InvalidParameterException("The --Database and the --Server options needs to be supplied when Write is enabled.");
                }
            }
        }

        public override void Run()
        {

            try
            {


                CheckOption();

                if (Test)
                {
                    var loader=new FileLoader();
                    loader.LoadFile(File);
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            //// connects to AF
            //AFDatabase afDatabase;
            //var afConnectionHelper = AfConnectionHelper.ConnectAndGetDatabase(Server, Database, out afDatabase);


        }







    }
}
