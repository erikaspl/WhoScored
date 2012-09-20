var migration = { };
var controllers;
migration.index =
    {
        initUrl: function (controllerUrl) {
            controllers = controllerUrl;           
        }
    };

var oFixturesListTable;
    
function S4() {
return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
 
function guidGenerator() {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

function migrateMatchDetails(matchId, matchRound) {
    season = $('#seasonSelect').val();
    series = $('#seriesSelect').val();
    $('#processingModal').modal('show');
    $.ajax({
        url: controllers.migrateMatchDetails,
        type: "POST",
        data:
            {
                matchId: matchId,
                season: season,
                matchRound: matchRound,
                leagueId: series
            },
        dataType: 'json',
        traditional: true,
        success: function() {
            oFixturesListTable.fnDraw();
        },
        complete: function() {
            $('#processingModal').modal('hide');
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
                        url: controllers.showFixtures,
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
                                                    return '<button class="btn btn-primary btn-mini" type="button" onclick="migrateMatchDetails(' + oObj.aData[0] + ', ' + oObj.aData[2] + ')">Migrate</button>'
                                              
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
        url: controllers.getMigrationStatus,
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

    $('#migrateFixturesModal').modal({
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
        var country = $('#countryId').val();

        $.ajax({
            url: controllers.migrateSeriesDetails,
            type: "POST",
            async: false,
            data:
                {
                    countryId: country
                },
            dataType: 'json',
            traditional: true
        });
    });

    $('#migrateFixtures').click(function() {
        var series = $('#seriesSelect').val();
        var season = $('#seasonSelect').val();

        $('#migrateFixturesModal').modal('show');

        $.ajax({
            url: controllers.migrateSeriesFixtures,
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
                    setFixturesDataTable();
                } else {
                    $("#fixturesMigrationFailure").show();
                    $("#fixturesMigrationSuccess").hide();
                }
            },                
            complete: function() {
                $('#migrateFixturesModal').modal('hide');                
            }            
        });
    });

    $('#migrateMatches').click(function() {

        var series = $('#seriesSelect').val();
        var season = $('#seasonSelect').val();
        var leagueId = $('#seriesSelect').val();
        var guid = guidGenerator();

        $('#migrationProgressBar').width("0%");
        $('#migrating').show();
        $('#migrationSuccess').hide();
        $('#migrationFailed').hide();
        $('#migrationProgressModalClose').hide();
        $('#migrationProgressModal').modal('show');

        matchMigrationInterval = setInterval(function() { checkMatchMigrationStatus(guid); }, 1000);

        $.ajax({
                url: controllers.startMigrateMatchDetails,
                type: "POST",
                data:
                    {
                        seriesId: series,
                        season: season,
                        leagueId: leagueId,
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
                        url: controllers.completeMigrateMatchDetails,
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
    });

    $("#resetDatabase").click(function () {
        $.ajax({
            url: controllers.resetDatabase,
            type: "POST",
            dataType: 'json',
            traditional: true
        });
    });


    $('#seasonSelect').change(function() {
        setFixturesDataTable();
    });
                      
    $('#seriesSelect').change(function() {
        setFixturesDataTable();
    });
};    
    
   