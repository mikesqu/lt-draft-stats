

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
            int totalStartedService)
        {
            // string prevStateColorCode;
            // if (prevState == "Online")
            // {
            //     prevStateColorCode = "rgb(56, 148, 54)";
            // }
            // else if (prevState == "NeedsAttention")
            // {
            //     prevStateColorCode = "rgb(197, 67, 68)";
            // }
            // else
            // {
            //     prevStateColorCode = "rgb(214, 186, 70)";
            // }

            // string currentStateColorCode;
            // if (currentState == "Online")
            // {
            //     currentStateColorCode = "rgb(56, 148, 54)";
            // }
            // else if (currentState == "NeedsAttention")
            // {
            //     currentStateColorCode = "rgb(197, 67, 68)";
            // }
            // else
            // {
            //     currentStateColorCode = "rgb(214, 186, 70)";
            // }


            string htmlString = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"">
                    <title>2025 šauktinių informacija</title>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                    <link rel=""stylesheet"" href=""/W3.CSS%20Template_files/w3.css"">
                    <link rel=""stylesheet"" href=""/W3.CSS%20Template_files/css.css"">
                    <link rel=""stylesheet"" href=""/W3.CSS%20Template_files/font-awesome.min.css"">
                    <style>
                    body, h1, h2, h3, h4, h5, h6 {{ font-family: ""Montserrat"", sans-serif; }}
                    .w3-row-padding img {{ margin-bottom: 12px; }}
                    .w3-sidebar {{ width: 120px; background: #222; }}
                    #main {{ margin-left: 120px; }}
                    @media only screen and (max-width: 600px) {{ 
                        #main {{ margin-left: 0; }} 
                    }}
                    </style>
                </head>
                <body class=""w3-black"">

                    <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">
                        <h3>2025 metų šauktiniu sąrašo informacija: </h3>
                    </div>
                    
                    <div class=""w3-row w3-center w3-padding-16 w3-section w3-light-grey"">
                        <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">25149</span><br>
                            Viso pašaukta
                        </div>
                         <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{totalAcceptable}</span><br>
                            Iš viso pripažinti tinkami
                        </div>
                         <div class=""w3-quarter w3-section"">
                            <span class=""w3-xlarge"">{totalStartedService}</span><br>
                            Iš viso pradėjo tarnybą
                        </div>
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
                    </div>

                    <div>
                        <p> Informacija atnaujinta: {updatedOn.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)} </p>
                    </div>
                </body>
                </html>";


            return htmlString;

        }

    }
}