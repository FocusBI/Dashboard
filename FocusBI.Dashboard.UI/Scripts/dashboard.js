var executionsTable;
var executablesTable;
var executablesWrapper;
var messagesTable;
var messagesWrapper;

var detailsTable;
var detailType;
var detailId;
var interval;

function UpdateTables() {
    $('.blockUI').block({
        message: '<h1>loading...</h1>',
        fadein: 200,
        fadeout: 400,
        overlayCSS: {
            backgroundColor: '#000',
            opacity: 0.45,
            cursor: 'wait'
        },
    });
    try {
        // this may timeout or error if connection string is not correct. So dont make second call if we fail
        if (UpdateKPI()) {
            UpdateProjectList();
            UpdateDetailTable(detailId, detailType);
        }
    }
    catch (error) {
        App.MsgDialog('type-warning', error);
    }
    window.setTimeout(function () { $('.blockUI').unblock(); }, 100); //without a delay we dont get the block ui mask....no idea why
}

$(document).ready(function () {

    // KPI section
    $("#updateKPI").off("click");
    $("#updateKPI").click(function () {
        UpdateTables();
    });

    $("#updateFequency").change(function () {
        if (interval) {
            clearInterval(interval);
        }
        var frequency = $(this).val();
        if (frequency > 0) {
            frequency = frequency * 1000;
            interval = setInterval(UpdateTables, frequency);
        }
    });

    $(".statusPanel").click(function () {
        //unselect any selected panels
        $(".statusPanel").removeClass("selected");
        //set selected style to this panel
        $(this).addClass("selected");
        var status = $(this).attr('data-status');
        FilterProjectList(status);
    });

    // Executions section
    // keep ref here so its not lost
    executablesWrapper = $("#executablesWrapper");
    messagesWrapper = $("#messagesWrapper");

    executionsTable = $('#executions').DataTable({
        "pageLength": 5,
        "bSort": true,
        "lengthMenu": [[2, 5, 10, 25, 50, -1], [2, 5, 10, 25, 50, "All"]],
        "order": [[0, "desc"]],
        rowId: 'ExecutionId',
        "columns": [
            { "data": "Id" },
            { "data": "ProjectName" },
            { "data": "PackageName" },
            { "data": "Status" },
            { "data": "RunDateString" },
            { "data": "StartTimeString" },
            { "data": "EndTimeString" },
            { "data": "ElapsedTimeInMinutes" },
            { "data": "NumberOfExecutables" },
            { "data": "NumberOfWarnings" },
            { "data": "NumberOfErrors" }
        ],
        "columnDefs": [
            {
                /* "render": function (data, type, row) {
                    return '<div class="statusPanel panel panel-warning>' + data + '</div>';
                },
               ,*/
                "fnCreatedCell": function (td, data, rowData, iRow, iCol) {
                    var className = 'info';
                    if (data == "failed")
                        className = 'danger';
                    if (data == "halted")
                        className = 'warning';
                    if (data == "succeeded")
                        className = '';
                    if(className != '')
                        $(td).addClass('text-' + className);
                },
                "targets": 3
            },
            {
                "render": function (data, type, row) {
                    return '<a>' + data + '</a>';
                },
                "fnCreatedCell": function (td, data, rowData, iRow, iCol) {
                    $(td).addClass('drilldown-control');
                    $(td).attr("data-id", rowData.Id);
                    $(td).data("type", "detail");
                },
                "targets": 8
            },
            {
                "render": function (data, type, row) {
                    if (data != 0) {
                        return '<a>' + data + '</a>';
                    }
                    else {
                        return data;
                    }
                },
                "fnCreatedCell": function (td, data, rowData, iRow, iCol) {
                    if (data != 0) {
                        $(td).addClass('drilldown-control');
                        $(td).data("id", rowData.Id);
                        $(td).data("type", "warning");
                    }
                },
                "targets": 9
            },
            {
                "render": function (data, type, row) {
                    if (data != 0) {
                        return '<a>' + data + '</a>';
                    }
                    else {
                        return data;
                    }
                },
                "fnCreatedCell": function (td, data, rowData, iRow, iCol) {
                    if (data != 0) {
                        $(td).addClass('drilldown-control');
                        $(td).data("id", rowData.Id);
                        $(td).data("type", "error");
                    }
                },
                "targets": 10
            }
        ]
    });

    $('.dataTables_filter input').attr('placeholder', 'Search')

    // Add event listener for opening and closing details
    $('#executions tbody').on('click', 'td.drilldown-control', function () {
        var id = ($(this).data("id"))
        var type = ($(this).data("type"))
        UpdateDetailTable(id, type);
    });
    UpdateTables();
});

function IntialiseExecutablesTable() {
    executablesTable = $('#executables').DataTable({
        "pageLength": 5,
        "lengthMenu": [[2, 5, 10, 25, 50, -1], [2, 5, 10, 25, 50, "All"]],
        "order": [[0, "asc"]],
        rowId: 'Id',
        "columns": [
            { "data": "Id" },
            { "data": "PackageName" },
            { "data": "Name" },
            { "data": "StartTimeString" },
            { "data": "EndTimeString" },
            { "data": "Duration" },
            { "data": "ExecutionResult" },
            { "data": "ExecutionValue" }
        ]
    });
}

function IntialiseMessagesTable() {
    messagesTable = $('#messages').DataTable({
        "pageLength": 5,
        "lengthMenu": [[2, 5, 10, 25, 50, -1], [2, 5, 10, 25, 50, "All"]],
        "order": [[0, "asc"]],
        rowId: 'Id',
        "columns": [
            { "data": "Id" },
            { "data": "MessageText" },
            { "data": "TimeString" },
            { "data": "Source" },
            { "data": "Component" }
        ]
    });
}

function FilterProjectList(status) {
    var statusText = status;
    status = status.toLowerCase();
    if (status == 'all') {
        status = '';
    }
    //filter the table
    executionsTable.column(3).search(status, false, true).draw(true);

    // add in status text to table header
    var statusLabel = $("#statusFilter");
    if (statusLabel) {
        statusLabel.remove();
    }

    var html = '<label id="statusFilter">&nbsp &nbsp' + statusText + ' Executions</label>';
    $(html).insertAfter('#executions_length label');
}

function GeExecutables(id) {
    var data = App.GetApiData("/Home/GetExecutables", { executionId: id });
    if (!executablesTable) {
        IntialiseExecutablesTable();
    }
    executablesTable.clear();
    executablesTable.rows.add(data);
    detailTable = executablesTable;
    return executablesWrapper;
}

function GetMessages(id, type) {
    var data = App.GetApiData("/Home/GetMessages", { executionId: id, type: type });
    if (!messagesTable) {
        IntialiseMessagesTable();
    }
    messagesTable.clear();
    messagesTable.rows.add(data);
    detailTable = messagesTable;
    return messagesWrapper;
}

function UpdateProjectList(control) {
    var data = App.GetApiData("/Home/GetExecutions", null);
    executionsTable.clear();
    if (data) {
        executionsTable.rows.add(data).draw();
    }
    return executablesWrapper;
}

function UpdateKPI() {
    //alert("Here");
    var data = App.GetApiData("/Home/GetKPI", null)
    if (data) {
        //reset all to 0
        $('.statusCount').html(0);
        $.each(data, function (i, item) {
            $('#' + item.ExecutionStatus).html(item.RowCount);
        });
        $("#lastUpdated").html("Updated at:" + App.FormatDate(new Date()));
        return true;
    }
    else {
        return false;
    }
}

function UpdateDetailTable(id, type) {
    if (!id) {
        return;
    }
    var table = $('#executions').DataTable();
    var element = $('td.drilldown-control[data-id=' + id + ']');
    if (element) {
        var tr = $(element).closest('tr');
        var row = table.row(tr);
        // This row is already open and we have clicked same column then close it
        if (row.child.isShown() && detailType == type) {
            row.child.hide();
            detailType = null;
            detailId = null;
        }
        else {
            // Open this row
            //close all open rows and set data to null as we cant share/reuse datatable
            $("#executions tbody tr").each(function (index) {
                var row = table.row(this);
                if (row.child()) {
                    row.child().hide();
                }
            });
            if (type == "detail") {
                var html = GeExecutables(id);
            }
            else {
                var html = GetMessages(id, type);
            }
            if (html) {
                row.child(html).show();
                if (type == "warning") {
                    $("#messageHeader").addClass("bg-warning");
                    $("#messageHeader").removeClass("bg-danger");
                }
                if (type == "error") {
                    $("#messageHeader").addClass("bg-danger");
                    $("#messageHeader").removeClass("bg-warning");
                }
                detailTable.columns.adjust().draw();
            }
            else {

                alert("No datatable HTML!");
            }
            detailType = type;
            detailId = id;
        }
    }
}