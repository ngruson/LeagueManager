﻿function sortListbox(listBox) {
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
            $('.modal .alert-danger').text(jqXHR.responseText);
            $('.modal .alert-danger').removeClass('invisible');
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