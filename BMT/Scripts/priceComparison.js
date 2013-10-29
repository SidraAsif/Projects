Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function EndRequestHandler(sender, args) {
        BindContextMenu();
    }
});

function GenerateGraph(data, legend) {
    var listOfLegends = legend.split('@');
    var listOfData = data.split('@');
    var linesSettings;

    var minValue = parseInt(0);
    var maxValue = parseFloat(0);

    // clear existing graph
    $("#jqChart").html("");

    // To hold the data
    var lines = [];

    var count = parseInt(0);
    for (var index = 0; index < listOfLegends.length; index++) {
        lines.push({
            type: 'line',
            title: listOfLegends[index],
            data: [listOfData[count], listOfData[count + 1], listOfData[count + 2], listOfData[count + 3], listOfData[count + 4]],
            labels: { stringFormat: '$%d', font: '12px sans-serif' },
            extendRangeToOrigin: false

        });

        if (parseFloat(listOfData[count + 4]) > maxValue) {
            maxValue = parseFloat(listOfData[count + 4]);
        }

        //if ((parseInt(listOfData[count]) < minValue || minValue == 0) && parseInt(listOfData[count]) != 0) {
        if ((parseInt(listOfData[count]) < minValue || minValue == 0)) {
            minValue = parseInt(listOfData[count]);
        }

        count = count + 5;

    }

    $('#jqChart').jqChart({
        title: {
            text: 'EHR/PM Total Cost of Ownership (TOC) - 5 Years Comparison',
            font: '18px sans-serif',
            lineWidth: 1

        },
        axes: [
                 {
                     type: 'category',
                     location: 'bottom',
                     categories: ['Year 1', 'Year 2', 'Year 3', 'Year 4', 'Year 5'],
                     title: { text: 'Years of Ownership', fillStyle: 'black' }
                 },
                         {
                             type: 'linear',
                             location: 'left',
                             title: { text: 'TOC ($)', fillStyle: 'black' },
                             minimum: minValue,
                             maximum: maxValue + parseInt((maxValue / 5))
                         }

                      ],
        series: lines
    });
    $('#jqChart').jqChart('update');

    setTimeout("$('.body-container-left-tree').css('height', $('.body-container-right').height());", 2000);
}

function print() {
  //  ClearMessage();
    window.open('PrintReport.aspx');
}

function ClearMessage() {
    ShowMessage("", "clear");
}

// function to use the customer user control on client side
function ShowMessage(message, type) {
    $('#MessageBox').removeClass();
    $('#MessageBox').addClass(type);
    $('#MessageBox p').html(message);
}

