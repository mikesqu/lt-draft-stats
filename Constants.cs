

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Microsoft.OpenApi.Expressions;

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
            int reg6Potential,
            int hasToProvideDataReg1s1,
            int hasToProvideDataUntilExactReg1s1,
            int draftProcedureInProgressReg1s1,
            int isAsignedAndNeedsToArriveReg1s1,
            int quicklyHasToContactAndArriveReg1s1,
            int hasToAttendMedicalScreeningReg1s1,
            int hasToAttendAdditionalMedScreeningReg1s1,
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s1,
            int inServiceReg1s1,
            int draftHasBeenPostponedReg1s1,
            int removedFromDraftReg1s1,
            int hasToProvideDataReg1s2,
            int hasToProvideDataUntilExactReg1s2,
            int draftProcedureInProgressReg1s2,
            int isAsignedAndNeedsToArriveReg1s2,
            int quicklyHasToContactAndArriveReg1s2,
            int hasToAttendMedicalScreeningReg1s2,
            int hasToAttendAdditionalMedScreeningReg1s2,
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s2,
            int inServiceReg1s2,
            int draftHasBeenPostponedReg1s2,
            int removedFromDraftReg1s2,
            int hasToProvideDataReg1s3,
            int hasToProvideDataUntilExactReg1s3,
            int draftProcedureInProgressReg1s3,
            int isAsignedAndNeedsToArriveReg1s3,
            int quicklyHasToContactAndArriveReg1s3,
            int hasToAttendMedicalScreeningReg1s3,
            int hasToAttendAdditionalMedScreeningReg1s3,
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s3,
            int inServiceReg1s3,
            int draftHasBeenPostponedReg1s3,
            int removedFromDraftReg1s3,
            int hasToProvideDataReg1s4,
            int hasToProvideDataUntilExactReg1s4,
            int draftProcedureInProgressReg1s4,
            int isAsignedAndNeedsToArriveReg1s4,
            int quicklyHasToContactAndArriveReg1s4,
            int hasToAttendMedicalScreeningReg1s4,
            int hasToAttendAdditionalMedScreeningReg1s4,
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s4,
            int inServiceReg1s4,
            int draftHasBeenPostponedReg1s4,
            int removedFromDraftReg1s4,
            int hasToProvideDataReg1s5,
            int hasToProvideDataUntilExactReg1s5,
            int draftProcedureInProgressReg1s5,
            int isAsignedAndNeedsToArriveReg1s5,
            int quicklyHasToContactAndArriveReg1s5,
            int hasToAttendMedicalScreeningReg1s5,
            int hasToAttendAdditionalMedScreeningReg1s5,
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s5,
            int inServiceReg1s5,
            int draftHasBeenPostponedReg1s5,
            int removedFromDraftReg1s5,
            int hasToProvideDataReg1s6,
            int hasToProvideDataUntilExactReg1s6,
            int draftProcedureInProgressReg1s6,
            int isAsignedAndNeedsToArriveReg1s6,
            int quicklyHasToContactAndArriveReg1s6,
            int hasToAttendMedicalScreeningReg1s6,
            int hasToAttendAdditionalMedScreeningReg1s6,
            int hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s6,
            int inServiceReg1s6,
            int draftHasBeenPostponedReg1s6,
            int removedFromDraftReg1s6)
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
                        <h3>2025 metų šauktinių sąrašo statistika: </h3>
                    </div>

                    <div>
                        <p> Statistika paskutinį kartą skaičiuota: {updatedOn.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)} </p>
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
                            <span class=""w3-xlarge"">{3865 - totalAcceptable}</span><br>
                            Iš viso trūksta
                        </div>
                         <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{totalStartedService}</span><br>
                            Iš viso pradėjo tarnybą
                        </div>
                    </div>

                    <h5 style=""font-weight: bold;"">Viso sąrašo statusų statistika</h5>

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
                            Išbraukti iš karinės įskaitos
                        </div>
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{25149 - hasToProvideData - hasToProvideDataUntilExact - draftProcedureInProgress - isAsignedAndNeedsToArrive - quicklyHasToContactAndArrive - hasToAttendMedicalScreening - hasToAttendAdditionalMedScreening - hasToProvideAddtionalInfoAfterAdditionalMedScreening - inService - draftHasBeenPostponed - removedFromDraft}</span><br>
                            Statusų likutis: 
                        </div>
                    </div>

                    <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">

                        <h5 style=""font-weight: bold;"">Šaukimo eiga pagal regionus</h5>
                        <div class=""simple-bar-chart"">

                            <div class=""item"" style=""--clr: #5EB344; --val: {reg1Perc}"">
                                <div class=""label"">Alytaus reg.</div>
                                <div class=""value"">{reg1Acceptable} / 338 | trūksta: {338 - reg1Acceptable} </div>
                            </div>

                            <div class=""item"" style=""--clr: #FCB72A; --val: {reg2Perc}"">
                                <div class=""label"">Kauno reg.</div>
                                <div class=""value"">{reg2Acceptable} / 879 | trūksta: {879 - reg2Acceptable}</div>
                            </div>

                            <div class=""item"" style=""--clr: #F8821A; --val: {reg3Perc}"">
                                <div class=""label"">Klaipėdos reg.</div>
                                <div class=""value"">{reg3Acceptable} / 658 | trūksta: {658 - reg3Acceptable}</div>
                            </div>

                            <div class=""item"" style=""--clr: #E0393E; --val: {reg4Perc}"">
                                <div class=""label"">Panevėžio reg.</div>
                                <div class=""value"">{reg4Acceptable} / 355 | trūksta: {355 - reg4Acceptable}</div>
                            </div>

                            <div class=""item"" style=""--clr: #E0393E; --val: {reg5Perc}"">
                                <div class=""label"">Šiaulių reg.</div>
                                <div class=""value"">{reg5Acceptable} / 490 | trūksta: {490 - reg5Acceptable}</div>
                            </div>

                            <div class=""item"" style=""--clr: #963D97; --val: {reg6Perc}"">
                                <div class=""label"">Vilniaus reg.</div>
                                <div class=""value"">{reg6Acceptable} / 1145 | trūksta: {1145 - reg6Acceptable}</div>
                            </div>
                        </div>


                        <h5 style=""font-weight: bold;"">Spėjamas NPPKTN eiliškumas pagal galimai tinkamus</h5>
                        <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">
                            <div class=""w3-quarter w3-section"">
                                <span class=""w3-xlarge"">{reg1Acceptable} / {reg1Potential}</span>
                                <br>
                                Alytaus reg.
                            </div>
                            <div class=""w3-quarter w3-section"">
                                <span class=""w3-xlarge"">{reg2Acceptable} / {reg2Potential}</span>
                                <br>
                                Kauno reg.
                            </div>
                            <div class=""w3-quarter w3-section"">
                                <span class=""w3-xlarge"">{reg3Acceptable} / {reg3Potential}</span>
                                <br>
                                Klaipėdos reg.
                            </div>
                            <div class=""w3-quarter w3-section"">
                                <span class=""w3-xlarge""> {reg4Acceptable} / {reg4Potential}</span>
                                <br>
                                Panevėžio reg.
                            </div>
                            <div class=""w3-quarter w3-section"">
                                <span class=""w3-xlarge"">{reg5Acceptable} / {reg5Potential}</span>
                                <br>
                                Šiaulių reg.
                            </div>
                            <div class=""w3-quarter w3-section"">
                                <span class=""w3-xlarge"">{reg6Acceptable} / {reg6Potential}</span>
                                <br>
                                Vilniaus reg.
                            </div>
                        </div>

                        <br/>
                        <br/>
                        <br/>

                        <h4 style=""font-weight: bold;"" >Distribucija Vilniaus regione:</h4>
                            <table id=""distrib-chart"" class=""charts-css column multiple stacked show-labels"">

                                <tbody>
                                    <tr>
                                        <th scope=""row""> 0-1257 </th>
                                        <td style=""--size: calc({hasToProvideDataReg1s1} / 1257);"">{hasToProvideDataReg1s1}</td>
                                        <td style=""--size: calc({hasToProvideDataUntilExactReg1s1} / 1257);"">{hasToProvideDataUntilExactReg1s1}</td>
                                        <td style=""--size: calc({draftProcedureInProgressReg1s1} / 1257);"">{draftProcedureInProgressReg1s1}</td>
                                        <td style=""--size: calc({isAsignedAndNeedsToArriveReg1s1} / 1257);"">{isAsignedAndNeedsToArriveReg1s1}</td>
                                        <td style=""--size: calc({quicklyHasToContactAndArriveReg1s1} / 1257);"">{quicklyHasToContactAndArriveReg1s1}</td>
                                        <td style=""--size: calc({hasToAttendMedicalScreeningReg1s1} / 1257);"">{hasToAttendMedicalScreeningReg1s1}</td>
                                        <td style=""--size: calc({hasToAttendAdditionalMedScreeningReg1s1} / 1257);"">{hasToAttendAdditionalMedScreeningReg1s1}</td>
                                        <td style=""--size: calc({hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s1} / 1257);"">{hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s1}</td>
                                        <td style=""--size: calc({inServiceReg1s1} / 1257);"">{inServiceReg1s1}</td>
                                        <td style=""--size: calc({draftHasBeenPostponedReg1s1} / 1257);"">{draftHasBeenPostponedReg1s1}</td>
                                        <td style=""--size: calc({removedFromDraftReg1s1} / 1257);"">{removedFromDraftReg1s1}</td>
                                    </tr>
                                    <tr>
                                        <th scope=""row""> 1257-2514 </th>
                                        <td style=""--size: calc({hasToProvideDataReg1s2} / 1257);"">{hasToProvideDataReg1s2}</td>
                                        <td style=""--size: calc({hasToProvideDataUntilExactReg1s2} / 1257);"">{hasToProvideDataUntilExactReg1s2}</td>
                                        <td style=""--size: calc({draftProcedureInProgressReg1s2} / 1257);"">{draftProcedureInProgressReg1s2}</td>
                                        <td style=""--size: calc({isAsignedAndNeedsToArriveReg1s2} / 1257);"">{isAsignedAndNeedsToArriveReg1s2}</td>
                                        <td style=""--size: calc({quicklyHasToContactAndArriveReg1s2} / 1257);"">{quicklyHasToContactAndArriveReg1s2}</td>
                                        <td style=""--size: calc({hasToAttendMedicalScreeningReg1s2} / 1257);"">{hasToAttendMedicalScreeningReg1s2}</td>
                                        <td style=""--size: calc({hasToAttendAdditionalMedScreeningReg1s2} / 1257);"">{hasToAttendAdditionalMedScreeningReg1s2}</td>
                                        <td style=""--size: calc({hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s2} / 1257);"">{hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s2}</td>
                                        <td style=""--size: calc({inServiceReg1s2} / 1257);"">{inServiceReg1s2}</td>
                                        <td style=""--size: calc({draftHasBeenPostponedReg1s2} / 1257);"">{draftHasBeenPostponedReg1s2}</td>
                                        <td style=""--size: calc({removedFromDraftReg1s2} / 1257);"">{removedFromDraftReg1s2}</td>            </tr>
                                    <tr>
                                        <th scope=""row""> 2514-3771 </th>
                                        <td style=""--size: calc({hasToProvideDataReg1s3} / 1257);"">{hasToProvideDataReg1s3}</td>
                                        <td style=""--size: calc({hasToProvideDataUntilExactReg1s3} / 1257);"">{hasToProvideDataUntilExactReg1s3}</td>
                                        <td style=""--size: calc({draftProcedureInProgressReg1s3} / 1257);"">{draftProcedureInProgressReg1s3}</td>
                                        <td style=""--size: calc({isAsignedAndNeedsToArriveReg1s3} / 1257);"">{isAsignedAndNeedsToArriveReg1s3}</td>
                                        <td style=""--size: calc({quicklyHasToContactAndArriveReg1s3} / 1257);"">{quicklyHasToContactAndArriveReg1s3}</td>
                                        <td style=""--size: calc({hasToAttendMedicalScreeningReg1s3} / 1257);"">{hasToAttendMedicalScreeningReg1s3}</td>
                                        <td style=""--size: calc({hasToAttendAdditionalMedScreeningReg1s3} / 1257);"">{hasToAttendAdditionalMedScreeningReg1s3}</td>
                                        <td style=""--size: calc({hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s3} / 1257);"">{hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s3}</td>
                                        <td style=""--size: calc({inServiceReg1s3} / 1257);"">{inServiceReg1s3}</td>
                                        <td style=""--size: calc({draftHasBeenPostponedReg1s3} / 1257);"">{draftHasBeenPostponedReg1s3}</td>
                                        <td style=""--size: calc({removedFromDraftReg1s3} / 1257);"">{removedFromDraftReg1s3}</td>

                                    </tr>
                                    <tr>
                                        <th scope=""row""> 3771-5028 </th>
                                        <td style=""--size: calc({hasToProvideDataReg1s4} / 1257);"">{hasToProvideDataReg1s4}</td>
                                        <td style=""--size: calc({hasToProvideDataUntilExactReg1s4} / 1257);"">{hasToProvideDataUntilExactReg1s4}</td>
                                        <td style=""--size: calc({draftProcedureInProgressReg1s4} / 1257);"">{draftProcedureInProgressReg1s4}</td>
                                        <td style=""--size: calc({isAsignedAndNeedsToArriveReg1s4} / 1257);"">{isAsignedAndNeedsToArriveReg1s4}</td>
                                        <td style=""--size: calc({quicklyHasToContactAndArriveReg1s4} / 1257);"">{quicklyHasToContactAndArriveReg1s4}</td>
                                        <td style=""--size: calc({hasToAttendMedicalScreeningReg1s4} / 1257);"">{hasToAttendMedicalScreeningReg1s4}</td>
                                        <td style=""--size: calc({hasToAttendAdditionalMedScreeningReg1s4} / 1257);"">{hasToAttendAdditionalMedScreeningReg1s4}</td>
                                        <td style=""--size: calc({hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s4} / 1257);"">{hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s4}</td>
                                        <td style=""--size: calc({inServiceReg1s4} / 1257);"">{inServiceReg1s4}</td>
                                        <td style=""--size: calc({draftHasBeenPostponedReg1s4} / 1257);"">{draftHasBeenPostponedReg1s4}</td>
                                        <td style=""--size: calc({removedFromDraftReg1s4} / 1257);"">{removedFromDraftReg1s4}</td>
                                    </tr>
                                    <tr>
                                        <th scope=""row""> 5028-6285 </th>
                                        <td style=""--size: calc({hasToProvideDataReg1s5} / 1257);"">{hasToProvideDataReg1s5}</td>
                                        <td style=""--size: calc({hasToProvideDataUntilExactReg1s5} / 1257);"">{hasToProvideDataUntilExactReg1s5}</td>
                                        <td style=""--size: calc({draftProcedureInProgressReg1s5} / 1257);"">{draftProcedureInProgressReg1s5}</td>
                                        <td style=""--size: calc({isAsignedAndNeedsToArriveReg1s5} / 1257);"">{isAsignedAndNeedsToArriveReg1s5}</td>
                                        <td style=""--size: calc({quicklyHasToContactAndArriveReg1s5} / 1257);"">{quicklyHasToContactAndArriveReg1s5}</td>
                                        <td style=""--size: calc({hasToAttendMedicalScreeningReg1s5} / 1257);"">{hasToAttendMedicalScreeningReg1s5}</td>
                                        <td style=""--size: calc({hasToAttendAdditionalMedScreeningReg1s5} / 1257);"">{hasToAttendAdditionalMedScreeningReg1s5}</td>
                                        <td style=""--size: calc({hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s5} / 190);"">{hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s5}</td>
                                        <td style=""--size: calc({inServiceReg1s5} / 1257);"">{inServiceReg1s5}</td>
                                        <td style=""--size: calc({draftHasBeenPostponedReg1s5} / 1257);"">{draftHasBeenPostponedReg1s5}</td>
                                        <td style=""--size: calc({removedFromDraftReg1s5} / 1257);"">{removedFromDraftReg1s5}</td>
                                    </tr>
                                    <tr>
                                        <th scope=""row""> 6285-7546 </th>
                                        <td style=""--size: calc({hasToProvideDataReg1s6} / 1261);"">{hasToProvideDataReg1s6}</td>
                                        <td style=""--size: calc({hasToProvideDataUntilExactReg1s6} / 1261);"">{hasToProvideDataUntilExactReg1s6}</td>
                                        <td style=""--size: calc({draftProcedureInProgressReg1s6} / 1261);"">{draftProcedureInProgressReg1s6}</td>
                                        <td style=""--size: calc({isAsignedAndNeedsToArriveReg1s6} / 1261);"">{isAsignedAndNeedsToArriveReg1s6}</td>
                                        <td style=""--size: calc({quicklyHasToContactAndArriveReg1s6} / 1261);"">{quicklyHasToContactAndArriveReg1s6}</td>
                                        <td style=""--size: calc({hasToAttendMedicalScreeningReg1s6} / 1261);"">{hasToAttendMedicalScreeningReg1s6}</td>
                                        <td style=""--size: calc({hasToAttendAdditionalMedScreeningReg1s6} / 1261);"">{hasToAttendAdditionalMedScreeningReg1s6}</td>
                                        <td style=""--size: calc({hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s6} / 1261);"">{hasToProvideAddtionalInfoAfterAdditionalMedScreeningReg1s6}</td>
                                        <td style=""--size: calc({inServiceReg1s6} / 1261);"">{inServiceReg1s6}</td>
                                        <td style=""--size: calc({draftHasBeenPostponedReg1s6} / 1261);"">{draftHasBeenPostponedReg1s6}</td>
                                        <td style=""--size: calc({removedFromDraftReg1s6} / 1261);"">{removedFromDraftReg1s6}</td>
                                    </tr>
                                </tbody>
                            </table>

                            <ul class=""charts-css legend legend-rectangle"" style=""margin-top: 6.5rem;"">
                                <li> Privalo pateikti duomenis </li>
                                <li> Privalo pateikti duomenis iki konkrečios datos </li>
                                <li> Šaukimo proceduros vykdomos </li>
                                <li> Privalo atvykti į nurodytą skyrių </li>
                                <li> Privalo skubiai susiekti arba atvykti į nurodytą skyrių </li>
                                <li> Privalo atvykti pasitikrinti sveikatos </li>
                                <li> Privalo atvykti papildomai pasitikrinti sveikatos </li>
                                <li> Privalo pateikti medicininius dokumentus po papildomo ištyrimo </li>
                                <li> Atlieka tarnybą </li>
                                <li> Privalomoji karo tarnyba atidėta </li>
                                <li> Išbraukti iš karinės įskaitos </li>
                            </ul>
                    </div>

                    
                </body>
                </html>";


            return htmlString;

        }

    }
}