

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace draft_data
{
    public static class DomainConstants
    {

        public static string GetPage
        (
            int hasToProvideData,
            int hasToProvideDataUntilExact,
            int draftProcedureInProgress,
            int isAsignedAndNeedsToArrive,
            int quicklyHasToContactAndArrive,
            int hasToAttendMedicalScreening,
            int draftHasBeenPostponed,
            int hasToAttendAdditionalMedScreening,
            int hasToProvideAddtionalInfoAfterAdditionalMedScreening,
            DateTime updatedOn,
            int inService,
            int totalAcceptable,
            int totalStartedService,
            int removedFromDraft,
            int reg1Acceptable,
            int reg2Acceptable,
            int reg3Acceptable,
            int reg4Acceptable,
            int reg5Acceptable,
            int reg6Acceptable,
            int reg1Potential,
            int reg2Potential,
            int reg3Potential,
            int reg4Potential,
            int reg5Potential,
            int reg6Potential)
        {

            int reg1Perc = (int)(((double)reg1Acceptable / 338) * 100);
            int reg2Perc = (int)(((double)reg2Acceptable / 879) * 100);
            int reg3Perc = (int)(((double)reg3Acceptable / 658) * 100);
            int reg4Perc = (int)(((double)reg4Acceptable / 355) * 100);
            int reg5Perc = (int)(((double)reg5Acceptable / 490) * 100);
            int reg6Perc = (int)(((double)reg6Acceptable / 1145) * 100);

            int reg1PercPot = (int)(((double)reg1Acceptable / reg1Potential) * 100);
            int reg2PercPot = (int)(((double)reg2Acceptable / reg2Potential) * 100);
            int reg3PercPot = (int)(((double)reg3Acceptable / reg3Potential) * 100);
            int reg4PercPot = (int)(((double)reg4Acceptable / reg4Potential) * 100);
            int reg5PercPot = (int)(((double)reg5Acceptable / reg5Potential) * 100);
            int reg6PercPot = (int)(((double)reg6Acceptable / reg6Potential) * 100);



            string htmlString = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"">
                    <title>2025 šauktinių statistika</title>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                    <link rel=""stylesheet"" href=""/W3.CSS%20Template_files/w3.css"">
                    <link rel=""stylesheet"" href=""/W3.CSS%20Template_files/css.css"">
                    <link rel=""stylesheet"" href=""/W3.CSS%20Template_files/font-awesome.min.css"">
                    <link rel=""stylesheet"" href=""/W3.CSS%20Template_files/extract.css"">
                    <link rel=""stylesheet"" href=""/bar.css"">
                </head>
                <body class=""w3-black"">

                    <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">
                        <h3>2025 metų šauktiniu sąrašo statistika: </h3>
                    </div>

                    <div>
                        <p> Informacija atnaujinta: {updatedOn.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)} </p>
                    </div>
                    
                    <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">25149</span><br>
                            Viso pašaukta
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">3865</span><br>
                            2025 m. šaukimo planas
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{totalAcceptable}</span><br>
                            Iš viso pripažinti tinkami (laukiantys priskyrimo)
                        </div>
                         <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{totalStartedService}</span><br>
                            Iš viso pradėjo tarnybą
                        </div>
                    </div>


                    <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{hasToProvideData}</span><br>
                            Privalo pateikti duomenis
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{hasToProvideDataUntilExact}</span><br>
                            Privalo pateikti duomenis iki konkrečios datos
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{draftProcedureInProgress}</span><br>
                            Šaukimo proceduros vykdomos
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{isAsignedAndNeedsToArrive}</span><br>
                            Privalo atvykti į nurodytą skyrių
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{quicklyHasToContactAndArrive}</span><br>
                            Privalo skubiai susiekti arba atvykti į nurodytą skyrių
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{hasToAttendMedicalScreening}</span><br>
                            Privalo atvykti pasitikrinti sveikatos
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{hasToAttendAdditionalMedScreening}</span><br>
                            Privalo atvykti papildomai pasitikrinti sveikatos
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{hasToProvideAddtionalInfoAfterAdditionalMedScreening}</span><br>
                            Privalo pateikti medicininius dokumentus po papildomo ištyrimo
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{inService}</span><br>
                            Atlieka tarnybą
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{draftHasBeenPostponed}</span><br>
                            Privalomoji karo tarnyba atidėta
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{removedFromDraft}</span><br>
                            išbraukti iš karinės įskaitos
                        </div>
                    </div>

                    <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">

                        <h5>Eiliškumas pagal planą</h5>
                        <div class=""simple-bar-chart"">

                            <div class=""item"" style=""--clr: #5EB344; --val: {reg1Perc}"">
                                <div class=""label"">Alytaus reg.</div>
                                <div class=""value"">{reg1Acceptable} / 338</div>
                            </div>

                            <div class=""item"" style=""--clr: #FCB72A; --val: {reg2Perc}"">
                                <div class=""label"">Kauno reg.</div>
                                <div class=""value"">{reg2Acceptable} / 879</div>
                            </div>

                            <div class=""item"" style=""--clr: #F8821A; --val: {reg3Perc}"">
                                <div class=""label"">Klaipėdos reg.</div>
                                <div class=""value"">{reg3Acceptable} / 658</div>
                            </div>

                            <div class=""item"" style=""--clr: #E0393E; --val: {reg4Perc}"">
                                <div class=""label"">Panevėžio reg.</div>
                                <div class=""value"">{reg4Acceptable} / 355</div>
                            </div>

                            <div class=""item"" style=""--clr: #E0393E; --val: {reg5Perc}"">
                                <div class=""label"">Šiaulių reg.</div>
                                <div class=""value"">{reg5Acceptable} / 490</div>
                            </div>

                            <div class=""item"" style=""--clr: #963D97; --val: {reg6Perc}"">
                                <div class=""label"">Vilniaus reg.</div>
                                <div class=""value"">{reg6Acceptable} / 1145</div>
                            </div>
                        </div>

                         <h5>Eiliškumas pagal galimai tinkamus</h5>
                        <div class=""simple-bar-chart"">

                            <div class=""item"" style=""--clr: #5EB344; --val: {reg1PercPot}"">
                                <div class=""label"">Alytaus reg.</div>
                                <div class=""value"">{reg1Acceptable} / {reg1Potential})</div>
                            </div>

                            <div class=""item"" style=""--clr: #FCB72A; --val: {reg2PercPot}"">
                                <div class=""label"">Kauno reg.</div>
                                <div class=""value"">{reg2Acceptable} / {reg2Potential}</div>
                            </div>

                            <div class=""item"" style=""--clr: #F8821A; --val: {reg3PercPot}"">
                                <div class=""label"">Klaipėdos reg.</div>
                                <div class=""value"">{reg3Acceptable} / {reg3Potential}</div>
                            </div>

                            <div class=""item"" style=""--clr: #E0393E; --val: {reg4PercPot}"">
                                <div class=""label"">Panevėžio reg.</div>
                                <div class=""value"">{reg4Acceptable} / {reg4Potential}</div>
                            </div>

                            <div class=""item"" style=""--clr: #E0393E; --val: {reg5PercPot}"">
                                <div class=""label"">Šiaulių reg.</div>
                                <div class=""value"">{reg5Acceptable} / {reg5Potential}</div>
                            </div>

                            <div class=""item"" style=""--clr: #963D97; --val: {reg6PercPot}"">
                                <div class=""label"">Vilniaus reg.</div>
                                <div class=""value"">{reg6Acceptable} / {reg6Potential}</div>
                            </div>
                        </div>

                    </div>

                    
                </body>
                </html>";


            return htmlString;

        }

    }
}