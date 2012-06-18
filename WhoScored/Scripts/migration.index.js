var oFixturesListTable;
    
function S4() {
return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
 
function guidGenerator() {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}
   
function migrateMatchDetails(matchId) {

    $('#processingModal').modal('show');
    $.ajax({
        url: '@Url.Action("MigrateMatchDetails", "Migration")',
        type: "POST",
        data:
            {
                matchId: matchId
            },
        dataType: 'json',
        traditional: true,
        success: function() {
            $('#processingModal').modal('hide');
            oFixturesListTable.fnDraw();
        }
    });
}

function closeMigrationProgressModal() {
    $('#migrationProgressModal').modal('hide');
    oFixturesListTable.fnDraw();
}

var series = null;
var season = null;

function setFixturesDataTable() {
       
        series = $('#seriesSelect').val();
        season = $('#seasonSelect').val();
        if (season != null && series != null) {
                    $.ajax({
                        url: '@Url.Action("ShowFixtures", "Migration")',
                        type: "POST",
                        data:
                            {
                                seriesId: series,
                                season: season
                            },
                        dataType: 'json',
                        traditional: true,
                        success: function(data) {
                            if (data == false) {
                                $("#fixturesAlert").show();
                                $('#example_wrapper').hide();
                            }
                            else {
                                $('#example').show();
                                $('#example_wrapper').show();
                                $("#fixturesAlert").hide();
                                $("#fixturesMigrationSuccess").hide();
                                $("#fixturesMigrationFailure").hide();

                                oFixturesListTable = $('#example').dataTable({
                                    "bRetrieve": true,
                                    "bServerSide": true,
                                    "bPaginate": false,
                                    "bLengthChange": false,
                                    "sAjaxSource": "Migration/AjaxHandler",
                                    "bProcessing": true,
                                    "fnServerData": function ( sSource, aoData, fnCallback ) {

                                        aoData.push({ "name": "season", "value": season });
                                        aoData.push({ "name": "seriesId", "value": series });

                                        $.ajax({
                                            "dataType": 'json',
                                            "type": "POST",
                                            "url": sSource,
                                            "data": aoData,
                                            "success": fnCallback
                                        });
                                    },
                                    "aoColumns": [
                                        {
                                            "sName": "MatchID",                                        
                                            "bSearchable": false,
                                            "bSortable": false,
                                            "fnRender": function(oObj) {
                                                if (oObj.aData[5] == 'False') {
                                                    return '<button class="btn btn-primary btn-mini" type="button" onclick="migrateMatchDetails(' + oObj.aData[0] + ')">Migrate</button>'
                                              
                                                }
                                                else {
                                                    return 'Match migrated';
                                                }
                                            }
                                        },
                                        { "sName": "MatchDate" },
                                        { "sName": "MatchRound" },
                                        { "sName": "HomeTeamName" },
                                        { "sName": "AwayTeamName" }                                
                                    ]
                                });

                                oFixturesListTable.fnDraw();
                            }
                        }
                    });
                }
}

var matchMigrationInterval;
function checkMatchMigrationStatus(operationId) {
    $.ajax({
        url: '@Url.Action("GetMigrationStatus", "Migration")',
        type: "POST",
        data:
            {
                operationId: operationId
            },
        dataType: 'json',
        traditional: true,
        success: function(result) {
                $('#migrationProgressBar').width(result + "%");
                }                
    });
}

function MigrationDocumentReady() {
    $('#processingModal').modal({
        keyboard: false,
        backdrop: false,
        show: false
    });

    $.extend( $.fn.dataTableExt.oStdClasses, {
        "sWrapper": "dataTables_wrapper form-inline"
    } );
       
    //Attach cascading behavior to the orderID select element.
    $("#seriesSelect").CascadingDropDown("#countryId", '/Migration/AsyncSeriesSelect');
    $("#seasonSelect").CascadingDropDown("#countryId", '/Migration/AsyncSeasonSelect');

    $('#migrateSeries').click(function() {
        var series = $('#seriesSelect').val();

        $.ajax({
            url: '@Url.Action("MigrateSeriesDetails", "Migration")',
            type: "POST",
            async: false,
            data:
                {
                    seriesId: series
                },
            dataType: 'json',
            traditional: true
        });
    });

    $('#migrateFixtures').click(function() {
        var series = $('#seriesSelect').val();
        var season = $('#seasonSelect').val();

        $.ajax({
            url: '@Url.Action("MigrateSeriesFixtures", "Migration")',
            type: "POST",
            data:
                {
                    seriesId: series,
                    season: season
                },
            dataType: 'json',
            traditional: true, 
            success: function(result) {
                if (result == true) {
                    $("#fixturesMigrationSuccess").show();
                    $("#fixturesMigrationFailure").hide();
                } else {
                    $("#fixturesMigrationFailure").show();
                    $("#fixturesMigrationSuccess").hide();
                }
            }
        });
    });

    $('#migrateMatches').click(function() {

        var series = $('#seriesSelect').val();
        var season = $('#seasonSelect').val();
        var guid= guidGenerator();
            
        $('#migrationProgressBar').width("0%");
        $('#migrating').show();
        $('#migrationSuccess').hide();
        $('#migrationFailed').hide();
        $('#migrationProgressModalClose').hide();
        $('#migrationProgressModal').modal('show');
            
        matchMigrationInterval = setInterval(function() { checkMatchMigrationStatus(guid); }, 1000);
            
        $.ajax({
                url: '@Url.Action("StartMigrateMatchDetails", "Migration")',
                type: "POST",
                data:
                    {
                        seriesId: series,
                        season: season,
                        operationId: guid
                    },
                dataType: 'json',
                traditional: true,
                success: function() {
                    $('#migrationProgressBar').width("100%");
                    setTimeout(function() {
                        $('#migrating').hide();
                        $('#migrationSuccess').show();
                        $('#migrationProgressModalClose').show();
                    }, 1000);
                            
                },
                error: function() {
                        $('#migrating').hide();
                        $('#migrationFailed').show();
                        $('#migrationProgressModalClose').show();
                },
                    
                complete: function() {                        
                    clearInterval(matchMigrationInterval);
                    $.ajax({
                        url: '@Url.Action("CompleteMigrateMatchDetails", "Migration")',
                        type: "POST",
                        data:
                            {
                                operationId: guid
                            },
                        dataType: 'json',
                        traditional: true
                    });
                }
            }
        );
    }
    );

    $('#seasonSelect').change(function() {

        setFixturesDataTable();
    });
                      
    $('#seriesSelect').change(function() {
        setFixturesDataTable();
    });
};    
    
   