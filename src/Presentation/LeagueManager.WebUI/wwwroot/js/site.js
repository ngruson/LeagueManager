function sortListbox(listBox) {
    var listitems = listBox.children('option').get();

    listitems.sort(function (a, b) {
        var compA = $(a).text().toUpperCase();
        var compB = $(b).text().toUpperCase();
        return compA < compB ? -1 : compA > compB ? 1 : 0;
    });

    $.each(listitems, function (idx, itm) {
        listBox.append(itm);
    });
}

function moveSelectedItems(src, trg) {
    var selectedOptions = $('option:selected', src);
    for (var i = 0; i < selectedOptions.length; i++) {
        var option = $("<option />").val(selectedOptions[i].value).html(selectedOptions[i].text);
        $(trg).append(option);
        selectedOptions[i].remove();
    }
    sortListbox(trg);
}

function getCountries(dropdown, url) {
    dropdown.empty();
    dropdown.append('<option selected="true" disabled>Choose Country</option>');
    dropdown.prop('selectedIndex', 0);

    $.getJSON(url, function (data) {
        $.each(data, function (key, entry) {
            dropdown.append($('<option></option>').attr('value', entry.code).text(entry.name));
        });
    });
}

function getTeams(listBox, selectedListBox, url) {
    listBox.empty();

    $.getJSON(url, function (data) {
        $.each(data, function (key, entry) {
            if ($('option[value=\'' + entry.name + '\']', selectedListBox).length === 0) {
                listBox.append($('<option></option>').attr('value', entry.name).text(entry.name));
            }
        });
    });
}

function createTeam(team, country, listBox, url) {
    var req = {
        name: $(team).val(),
        country: $(country).text()
    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        data: JSON.stringify(req),
        success: function () {
            var teamName = $(team).val();
            var option = $("<option />").val(teamName).html(teamName);
            listBox.append(option);
            sortListbox(listBox);

            $('.modal .alert-success').text('Team \"' + teamName + '\" has been created');
            $('.modal .alert-success').removeClass('invisible');
            $('.modal .alert-danger').addClass('invisible');
            $(team).val('');
        },
        error: function (jqXHR) {
            $('.alert-danger').text(jqXHR.responseText);
            $('.alert-danger').removeClass('invisible');
        }
    });
}

function createTeamLeague(leagueName, selectedTeams) {
    var teams = [];
    for (var i = 0; i < $(selectedTeams).length; i++) {
        teams.push($(selectedTeams)[i].value);
    }

    var req = {
        name: $(leagueName).val(),
        teams: teams
    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        data: JSON.stringify(req),
        success: function () {
            $('#create-league-alert-success').text('League \"' + teamName + '\" has been created');
            $('.modal .alert-success').removeClass('invisible');
            $('.modal .alert-danger').addClass('invisible');
            $(team).val('');
        },
        error: function (jqXHR) {
            $('.modal .alert-danger').text(jqXHR.responseText);
            $('.modal .alert-danger').removeClass('invisible');
        }
    });
}

function getRounds(dropdown, arr) {
    dropdown.empty();

    for (var i = 0; i < arr.length; i++) {
        var option = $("<option />").val(arr[i]).html(arr[i]);
        dropdown.append(option);
    }

    dropdown.prop('selectedIndex', 0);
}

async function updateTeamLeagueMatch(url, homeTeam, awayTeam, startTime) {
    var req = {
        homeTeam: homeTeam,
        awayTeam: awayTeam,
        startTime: startTime
    };

    var result = await $.ajax({
        type: "PUT",
        url: url,
        data: req
    });
    return result;
}

async function updateTeamLeagueMatchScore(url, homeTeam, homeScore, awayTeam, awayScore) {
    var req = {
        homeMatchEntry: {
            team: {
                name: homeTeam
            },
            homeAway: "Home",
            score: {
                value: homeScore
            }
        },
        awayMatchEntry: {
            team: {
                name: awayTeam
            },
            homeAway: "Away",
            score: {
                value: awayScore
            }
        }
    };

    var result = await $.ajax({
        type: "PUT",
        url: url,
        data: req
    });
    return result;
}

/*ViewMatchDetails */

async function createPlayer(firstName, middleName, lastName, url) {
    var req = {
        firstName: $(firstName).val(),
        middleName: $(middleName).val(),
        lastName: $(lastName).val()
    };
    var result = false;

    await $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        data: JSON.stringify(req),
        success: function () {
            result = true;
        },
        error: function (jqXHR) {
            $('.modal .alert-danger').text(jqXHR.responseText);
            $('.modal .alert-danger').removeClass('invisible');
            result = false;
        }
    });

    return result;
}

function constructFullName(firstName, middleName, lastName) {
    var result = firstName;
    if (middleName)
        result += ' ' + middleName;
    result += ' ' + lastName;
    return result;
}

async function addPlayerToTeamCompetitor(leagueName, teamName, playerNumber, playerName, url) {
    var req = {
        "leagueName": leagueName,
        "teamName": teamName,
        "playerNumber": playerNumber,
        "playerName": playerName
    };
    var result = false;

    await $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        data: JSON.stringify(req),
        success: function () {
            result = true;
        },
        error: function (jqXHR) {
            $('.modal .alert-danger').text(jqXHR.responseText);
            $('.modal .alert-danger').removeClass('invisible');
            result = false;
        }
    });

    return result;
}

async function addPlayerToLineup(number, playerName, url) {
    var req = {
        number: $(number).val(),
        player: playerName
    };
    var result = false;

    await $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        data: JSON.stringify(req),
        success: function () {
            result = true;
        },
        error: function (jqXHR) {
            $('.modal .alert-danger').text(jqXHR.responseText);
            $('.modal .alert-danger').removeClass('invisible');
            result = false;
        }
    });

    return result;
}

async function getPlayerNumber(url) {
    var number = '';

    await $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        success: function (data) {
            number = data.number;
        }
    });

    return number;
}

async function updateTeamLeagueMatchLineupEntry(url, playerNumber, playerName) {
    var req = {
        playerNumber: playerNumber,
        playerName: playerName
    };

    var result = await $.ajax({
        type: "PUT",
        url: url,
        data: req
    });
    return result;
}

function getPlayers(dropdowns, url) {
    dropdowns.each(function() {
        var dropdown = $(this);
        dropdown.empty();
        dropdown.append('<option selected="true" disabled>--Select player--</option>');
        dropdown.prop('selectedIndex', 0);

        $.getJSON(url, function (data) {
            $.each(data, function (key, entry) {
                dropdown.append($('<option></option>').attr('value', entry.player.fullName).text(entry.player.fullName));
            });
        });
    });
}