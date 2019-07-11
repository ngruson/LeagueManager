function sortListbox(fieldname) {
    var list = $('#' + fieldname);
    var listitems = list.children('option').get();

    listitems.sort(function (a, b) {
        var compA = $(a).text().toUpperCase();
        var compB = $(b).text().toUpperCase();
        return (compA < compB) ? -1 : (compA > compB) ? 1 : 0;
    });

    $.each(listitems, function (idx, itm) {
        list.append(itm);
    });
}