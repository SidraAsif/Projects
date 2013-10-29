var countControl = parseInt(1);

// Register Handlder On Page Load
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {

    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function BeginRequestHandler(sender, args) {
        TrackActiveSystem();
        EnableValidation();
        $('.body-container-right').css("min-height", "500px");
       // $('.body-container-left-tree').css('height', $('.body-container-right').height());
    }

    function EndRequestHandler(sender, args) {
        SwapRow();

        $("#btnTriggerCalculatePrice").live('click', function () {
            ClearMessage();
        });

        TrackActiveSystem();
        EnableValidation();
        $('.body-container-right').css("min-height", "500px");
        //$('.body-container-left-tree').css('height', $('.body-container-right').height());

        $("table#tableFeeControl img").hide();

        BindContextMenu(); 
    }

});

// Window Handler
// save voucher when user press enter
$(window).keyup(function (e) {
    var key = e.which;
    if (key == 13) {
        $("#btnSaveAndContinue")[0].click();
    }
    /*else
    e.preventDefault();*/

});

// On Document Ready
$(document).ready(function () {

    // interchange Recurring and onetime Fee fields
    SwapRow();

    // Clear Message
    $("#btnTriggerCalculatePrice").live('click', function () {
        ClearMessage();
    });

    $("#btnAddEHR").live('click', function () {
        $("#btnTriggerAddEHR")[0].click();
    });

    TrackActiveSystem();
    EnableValidation();

    $("table#tableFeeControl img").hide();

    $(".imgRemove").attr("style", "display:inline");

});

// function to calculate the Price
function CalculatePrice() {
    $('div#MessageBox').hide();
    
    FetchingValues();  

    var IsQtyValid = ValidateQty();
    var isClientRowValid = ValidateClientRows();

    if (Page_ClientValidate("")) {// page validation
        if (isClientRowValid) { // validate client rows input
            if (IsQtyValid) { // validate QTY in case of purchase model
                $("#btnTriggerCalculatePrice")[0].click();
            }
            else {
                $('#ctl00_bodyContainer_PriceCalculator_message_vSummary').attr("style", "display:inline");
                $('#ctl00_bodyContainer_PriceCalculator_message_vSummary').attr("style", "color:Red");
                $('#ctl00_bodyContainer_PriceCalculator_message_vSummary').html("<ul><li>The number of providers defined for One Time Fees do not match those defined for Recurring Fees.</li></ul>");
                $('#ControlOneTime411').focus();

                $('html,body').animate({
                    scrollTop: $(".body-container-right").offset().top
                }, 0);
            }
        }
        else {
            $('html,body').animate({
                scrollTop: $(".body-container-right").offset().top
            }, 0);


            // Enable validation summary for custome validation
            $('#ctl00_bodyContainer_PriceCalculator_message_vSummary').attr("style", "display:inline");
            $('#ctl00_bodyContainer_PriceCalculator_message_vSummary').attr("style", "color:Red");
        }
    }

}

// function to validate the Quantity
function ValidateQty() {
    var rbselectedQuestionnaire = parseInt(0);
    $(".selection :checked").each(function () {
        rbselectedQuestionnaire = parseInt($(this).val());
    });

    if (rbselectedQuestionnaire == 4) {
        var totalOTPhyQty = parseInt(0);
        var totalOTMLQty = parseInt(0);
        var totalOTPTQty = parseInt(0);
        var totalOTDQty = parseInt(0);
        var totalOTOQty = parseInt(0);

        var totalOGPhyQty = parseInt(0);
        var totalOGMLQty = parseInt(0);
        var totalOGPTQty = parseInt(0);
        var totalOGDQty = parseInt(0);
        var totalOGOQty = parseInt(0);

        // calculate onetime Qty
        $(".OneTimeFees :input.txt-quantity").each(function () {
            var controlSuffix = ParseNumFromString(this.id);

            controlSuffix = parseInt(controlSuffix) + 10;

            if (this.value.length != 0) {

                var selectedValue = $('#ControlOneTime' + controlSuffix + ' option:selected').val();


                if (selectedValue == undefined) {
                    controlSuffix = controlSuffix - 9;
                    selectedValue = $("#generic" + controlSuffix + " option:selected").val();
                }

                selectedValue = parseInt(selectedValue);

                if (selectedValue == 1)
                    totalOTPhyQty = totalOTPhyQty + parseInt(this.value);
                else if (selectedValue == 2)
                    totalOTMLQty = totalOTMLQty + parseInt(this.value);
                else if (selectedValue == 3)
                    totalOTPTQty = totalOTPTQty + parseInt(this.value);
                else if (selectedValue == 4)
                    totalOTDQty = totalOTDQty + parseInt(this.value);
                else if (selectedValue == 5)
                    totalOTOQty = totalOTOQty + parseInt(this.value);


            }

        });

        // calculate Recurring fee Qty
        $(".OngoingFees :input.txt-quantity").each(function () {
            var controlSuffix = ParseNumFromString(this.id);

            controlSuffix = parseInt(controlSuffix) + 10;

            if (this.value.length != 0) {

                var selectedValue = $('#ControlOnGoing' + controlSuffix + ' option:selected').val();

                if (selectedValue == undefined) {
                    controlSuffix = controlSuffix - 9;
                    selectedValue = $("#generic" + controlSuffix + " option:selected").val();
                }

                selectedValue = parseInt(selectedValue);

                if (selectedValue == 1)
                    totalOGPhyQty = totalOGPhyQty + parseInt(this.value);
                else if (selectedValue == 2)
                    totalOGMLQty = totalOGMLQty + parseInt(this.value);
                else if (selectedValue == 3)
                    totalOGPTQty = totalOGPTQty + parseInt(this.value);
                else if (selectedValue == 4)
                    totalOGDQty = totalOGDQty + parseInt(this.value);
                else if (selectedValue == 5)
                    totalOGOQty = totalOGOQty + parseInt(this.value);

            }
        });

        if (totalOTPhyQty == totalOGPhyQty && totalOTPTQty == totalOGPTQty && totalOTMLQty == totalOGMLQty &&
         totalOTDQty == totalOGDQty && totalOTOQty == totalOGOQty) {
            return true;
        }
        else
            return false;
    }
    else
        return true;
}

// Function validate Row
function ValidateClientRows() {
    var response = true;
    var messages = "";

    var rbselectedQuestionnaire = parseInt(0);
    $(".selection :checked").each(function () {
        rbselectedQuestionnaire = parseInt($(this).val());
    });



    $(".OneTimeFees :input.txt-Amount").each(function () {
        var controlSuffix = ParseNumFromString(this.id);

        if (this.id.indexOf("ControlOneTime") != -1) {

            if (this.value.length != 0) {

                controlSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) + 1) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);
                var selectedValue = $('#ControlOneTime' + controlSuffix + ' option:selected').val();
                selectedValue = parseInt(selectedValue);

                // Check Provider exists or not
                if (selectedValue == 0) {

                    messages = messages + "<ul><li>Please select payment method for '" + $('#ControlOneTime' + controlSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

                var typeSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) - 2) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);

                // Check Type if Available
                if (($('#ControlOneTime' + typeSuffix + ' option:selected').val() == 0) || ($('#ControlOneTime' + typeSuffix).val() == "")) {

                    messages = messages + "<ul><li>Please select type for '" + $('#ControlOneTime' + typeSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

            }
        }

        else if (this.id.indexOf("generic") != -1) {

            if (this.value.length != 0) {

                controlSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) + 1) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);

                var selectedValue = $('#generic' + controlSuffix + ' option:selected').val();
                selectedValue = parseInt(selectedValue);

                // Check Provider exists or not
                if (selectedValue == 0) {

                    messages = messages + "<ul><li>Please select payment method for '" + $('#generic' + controlSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

                var typeSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) - 2) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);

                // Check Type if Available
                if (($('#generic' + typeSuffix + ' option:selected').val() == 0) || ($('#generic' + typeSuffix).val() == "")) {

                    messages = messages + "<ul><li>Please select type for '" + $('#generic' + typeSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

            }
        }
    });


    $(".OngoingFees :input.txt-Amount").each(function () {
        var controlSuffix = ParseNumFromString(this.id);

        if (this.id.indexOf("ControlOnGoing") != -1) {

            if (this.value.length != 0) {

                controlSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) + 1) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);
                var selectedValue = $('#ControlOnGoing' + controlSuffix + ' option:selected').val();
                selectedValue = parseInt(selectedValue);

                // Check Provider exists or not
                if (selectedValue == 0) {

                    messages = messages + "<ul><li>Please select payment method for '" + $('#ControlOnGoing' + controlSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

                var typeSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) - 2) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);

                // Check Type if Available
                if (($('#ControlOnGoing' + typeSuffix + ' option:selected').val() == 0) || ($('#ControlOnGoing' + typeSuffix).val() == "")) {

                    messages = messages + "<ul><li>Please select type for '" + $('#ControlOnGoing' + typeSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

            }
        }

        else if (this.id.indexOf("generic") != -1) {

            if (this.value.length != 0) {

                controlSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) + 1) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);

                var selectedValue = $('#generic' + controlSuffix + ' option:selected').val();
                selectedValue = parseInt(selectedValue);

                // Check Provider exists or not
                if (selectedValue == 0) {

                    messages = messages + "<ul><li>Please select payment method for '" + $('#generic' + controlSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

                var typeSuffix = controlSuffix.toString().substr(0, 1) + (parseInt(controlSuffix.toString().substr(1, 1)) - 2) + controlSuffix.toString().substr(2, controlSuffix.toString().length - 2);

                // Check Type if Available
                if (($('#generic' + typeSuffix + ' option:selected').val() == 0) || ($('#generic' + typeSuffix).val() == "")) {

                    messages = messages + "<ul><li>Please select type for '" + $('#generic' + typeSuffix).parent().parent().children(":first").children("span:first").html() + "'.</li></ul>";
                    response = false;
                }

            }
        }
    });

    if (rbselectedQuestionnaire == 4) {

        $(".OneTimeFees :input.txt-quantity").each(function () {
            var controlSuffix = ParseNumFromString(this.id);
            controlSuffix = parseInt(controlSuffix) + 1;

            if (this.id.indexOf("generic") != -1) {

                if (this.value.length != 0) {
                    var selectedValue = $('#generic' + controlSuffix + ' option:selected').val();
                    selectedValue = parseInt(selectedValue);

                    // Check Provider exists or not
                    if (selectedValue == 0) {
                        messages = messages + "<ul><li>Provider of License field is required.</li></ul>";
                        response = false;
                    }

                    // Amount
                    controlSuffix = parseInt(controlSuffix) + 1;
                    selectedValue = $('#generic' + controlSuffix).val();
                    if (selectedValue.length == 0) {
                        messages = messages + "<ul><li>Amount of License field is required.</li></ul>";
                        response = false;
                    }
                }
                else {

                    messages = messages + "<ul><li>Qty of License field is required.</li></ul>";
                    response = false;
                }
            }
        });

        $(".OngoingFees :input.txt-quantity").each(function () {
            var controlSuffix = ParseNumFromString(this.id);
            controlSuffix = parseInt(controlSuffix) + 1;

            if (this.id.indexOf("generic") != -1) {
                if (this.value.length != 0) {
                    var selectedValue = $('#generic' + controlSuffix + ' option:selected').val();
                    selectedValue = parseInt(selectedValue);

                    // Check Provider exists or not
                    if (selectedValue == 0) {
                        messages = messages + "<ul><li>Provider of Maintenance field is required.</li></ul>";
                        response = false;
                    }

                    // Amount
                    controlSuffix = parseInt(controlSuffix) + 1;
                    selectedValue = $('#generic' + controlSuffix).val();
                    if (selectedValue.length == 0) {
                        messages = messages + "<ul><li>Amount of Maintenance field is required.</li></ul>";
                        response = false;
                    }

                    // Payment Method
                    controlSuffix = parseInt(controlSuffix) + 1;
                    selectedValue = $('#generic' + controlSuffix).val();
                    selectedValue = parseInt(selectedValue);

                    if (selectedValue == 0) {
                        messages = messages + "<ul><li>Payment Method of Maintenance field is required.</li></ul>";
                        response = false;
                    }
                }
                else {
                    messages = messages + "<ul><li>Qty of Maintenance field is required.</li></ul>";
                    response = false;
                }
            }
        });
    }
    else {
        $(".OngoingFees :input.txt-quantity").each(function () {
            var controlSuffix = ParseNumFromString(this.id);
            controlSuffix = parseInt(controlSuffix) + 1;

            if (this.id.indexOf("generic") != -1) {
                if (this.value.length != 0) {
                    var selectedValue = $('#generic' + controlSuffix + ' option:selected').val();
                    selectedValue = parseInt(selectedValue);

                    // Check Provider exists or not
                    if (selectedValue == 0) {
                        messages = messages + "<ul><li>Provider of Subscription field is required.</li></ul>";
                        response = false;
                    }

                    // Amount
                    controlSuffix = parseInt(controlSuffix) + 1;
                    selectedValue = $('#generic' + controlSuffix).val();
                    if (selectedValue.length == 0) {
                        messages = messages + "<ul><li>Amount of Subscription field is required.</li></ul>";
                        response = false;
                    }

                    // Payment Method
                    controlSuffix = parseInt(controlSuffix) + 1;
                    selectedValue = $('#generic' + controlSuffix).val();
                    selectedValue = parseInt(selectedValue);

                    if (selectedValue == 0) {
                        messages = messages + "<ul><li>Payment Method of Subscription field is required.</li></ul>";
                        response = false;
                    }
                }
                else {
                    messages = messages + "<ul><li>Qty of Subscription field is required.</li></ul>";
                    response = false;
                }
            }
        });
    }

    $('#ctl00_bodyContainer_PriceCalculator_message_vSummary').html(messages);
    return response; // if function complete then return true

}

// save client side control values to get on server side
function FetchingValues() {

    var tables = $('#hdnTablesId').val().split('@');
    for (var index = 0; index < tables.length - 1; index++) {
        var args = tables[index].split(',');
        for (var value = 0; value < args.length - 1; value++) {
            var tableId = args[value];
            var hdnFieldId = args[value + 1];
            var receivedValue = "";

            $('#' + tableId + ' tr').each(function (index) {

                $(this).find(':input[type=text]').each(function () {
                    receivedValue = receivedValue + $(this).val() + ",";
                });

                $(this).find(':selected').each(function () {
                    receivedValue = receivedValue + $(this).val() + ",";
                });

                receivedValue = receivedValue + "@";

                // remove un-necessary seperators
                receivedValue = receivedValue.replace(",@@", "@");
                receivedValue = receivedValue.replace("@@", "@");
                receivedValue = receivedValue.replace(",@", "@");

            });

            if (receivedValue.length > 0)
                $("#" + hdnFieldId).val(receivedValue);
        }
    }
}

// get static data by parsing for payment methods and type etc...
function ParseValues(controlType, selectedValue, title) {
    // Up-front purchase model - License (Onetime fee)
    if (title == "License:" && controlType.indexOf("Type") == -1)
        selectedValue = "1"; // Provider

    var options = $('#' + controlType).val();
    var items = options.split('@');
    var listItem = "";
    for (var index = 0; index < items.length - 1; index++) {
        var values = items[index].split(',');
        for (var indexItem = 0; indexItem < values.length - 1; indexItem++) {
            if (values[indexItem + 1].indexOf("Practice") != -1 && (title == "Subscription:" || title == "Maintenance:"))
                continue;
            else {
                if (selectedValue == values[indexItem])
                    listItem = listItem + "<option value=" + values[indexItem] + " selected='selected'>" + values[indexItem + 1] + "</option>";
                else
                    listItem = listItem + "<option value=" + values[indexItem] + ">" + values[indexItem + 1] + "</option>";
            }
        }
    }

    return listItem;
}

function ParseNumFromString(value) {
    return value.replace(/[^0-9\\.]/g, '').replace(/^(\d*\.\d*)\..*$/, "$1") * 1.0;
}

function SwapRow() {
    $(".selection :checked").each(function () {
        var selectedValue = $(this).val();
        if (parseInt(selectedValue) == 4) {
            var Row1 = $("#Row1").html();
            var Row4 = $("#Row4").html();
            $("#Row1").html(Row4);
            $("#Row4").html(Row1);

            var Row3 = $("#Row3").html();
            var Row6 = $("#Row6").html();
            $("#Row3").html(Row6);
            $("#Row6").html(Row3);
        }
    });
}

function SwitchSystem(systemId) {
    $("#hdnSystemId").val(systemId);
    $(".tabs li:not(.inactiveTab)").removeClass();
    $("#btnTriggerSwitchSystem")[0].click();
}

function TrackActiveSystem() {
    var systemId = $("#hdnSystemId").val();
    $(".tabs li:not(.inactiveTab)").removeClass();

    if (systemId == "" || parseInt(systemId) == 0) {
        $(".tabs li#1").addClass('activeTab');
    }
    else {
        $(".tabs li#" + systemId).addClass('activeTab');
    }
}

// function to delete the system
function DeleteSystem() {
    apprise('Are you sure want to delete the current system?', { 'verify': true }, function (response) {
        if (response)
            $("#btnTriggerDeleteSystem")[0].click();
    });
}

function EnableValidation() {
    $('.txt-quantity').numeric({
        decimal: false, negative: false
    }, function () {
        alert('Positive integers only'); this.value = ''; this.focus();
    });

    $('.txt-Amount').numeric({ negative: false });

    $('.txt-Amount').blur(function () {
        if (this.value.length > 0) {
            var amt = parseFloat(this.value);
            $(this).val(amt.toFixed(2));
        }
    });


    $('.txt-quantity').maxlength({
        events: [], // Array of events to be triggerd   
        maxCharacters: 4, // Characters limit  
        status: false, // True to show status indicator bewlow the element   
        statusClass: "status", // The class on the status div 
        statusText: "character left", // The status text 
        notificationClass: "notification",  // Will be added when maxlength is reached 
        showAlert: false, // True to show a regular alert message   
        alertText: "You have typed too many characters.", // Text in alert message  
        slider: false // True Use counter slider   
    });

    $('.txt-Amount').maxlength({
        events: [], // Array of events to be triggerd   
        maxCharacters: 10, // Characters limit  
        status: false, // True to show status indicator bewlow the element   
        statusClass: "status", // The class on the status div 
        statusText: "character left", // The status text 
        notificationClass: "notification",  // Will be added when maxlength is reached 
        showAlert: false, // True to show a regular alert message   
        alertText: "You have typed too many characters.", // Text in alert message  
        slider: false // True Use counter slider   
    });

    $('table td img.imgRemove').click(function () {
        $(this).parent().parent().remove();
    });

}

function ClearContents() {
    apprise('Are you sure want to clear the contents?', { 'verify': true }, function (response) {
        if (response) {
                    $("#btnTriggerClearContents")[0].click();
        }
    });
}

// Discard Changes confirmation
function DiscardChanges() {
    apprise('Are you sure want to discard the changes?', { 'verify': true }, function (response) {
        if (response)
            $("#btnTriggerDiscardChanges")[0].click();
    });
}

function ClearMessage() {
    ShowMessage("", "");
}

// function to use the customer user control on client side
function ShowMessage(message, type) {
    $('div#MessageBox').removeClass();
    $('div#MessageBox').addClass(type);
    $('div#MessageBox p').html(message);
}

// Generate dynamic control on client side
function GenerateControl(tableId, colspan, title, hiddenControlId, quantity, type, amount, paymentMethod) {
    var cell0 = $('#' + tableId).parents().parents('table').find("td").eq(0).width();
    var cell1 = $('#' + tableId).parents().parents('table').find("td").eq(1).width();
    var cell2 = $('#' + tableId).parents().parents('table').find("td").eq(2).width();
    var cell3 = $('#' + tableId).parents().parents('table').find("td").eq(3).width();
    var cell4 = $('#' + tableId).parents().parents('table').find("td").eq(4).width();

    // set table width
    $('#' + tableId).width("133.5%");

    var genericControlId = "generic";

    var controlCount = parseInt(colspan);
    var browser = navigator.appName;

    var rbselectedQuestionnaire = parseInt(0);
    $(".selection :checked").each(function () {
        rbselectedQuestionnaire = parseInt($(this).val());

    });

    var img = "<img src='../Themes/Images/delete.png' alt='Remove' title='Remove' class='imgRemove' />";
    var row = "";

    // Create row with different setting for all browser : Temporary solution
    if (browser == "Microsoft Internet Explorer") {

        if (rbselectedQuestionnaire == 3)
            $("#Row2").css("padding-left", "500px");
        else
            $("#Row2, #Row5").css("padding-left", "500px");

        if (controlCount == 4) { // If no quantity exists
            row = "<tr><td style='width:25%; min-width:25%; max-width:25%'>" + img + "<span>" + title + "</span></td>";

            row = row + "<td style='width:14.5%; min-width:14.5%; max-width:14.5%'><span class='label-fee-title'>Qty:</span><input id='" + genericControlId + countControl + "1' class='txt-quantity' type='text' value='" + quantity + "' /></td>";
            if (title.indexOf("Interface") != -1)
                row = row + "<td style='width:29%; min-width:29%; max-width:29%'><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnInterfaceType", type, title) + "</select></td>";
            else if (title.indexOf("Other") != -1)
                row = row + "<td style='width:27%; min-width:27%; max-width:27%'><span class='label-fee-title'>Type:</span><input id='" + genericControlId + countControl + "2' class='ddl-fee' type='text' value='" + type + "' /></td>";
            else
                row = row + "<td style='width:27%; min-width:27%; max-width:27%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnType", type, title) + "</select></td>";

            row = row + "<td align='left' style='width:9%; min-width:9%; max-width:9%'><input id='" + genericControlId + countControl + "3' class='txt-Amount' type='text' value='" + amount + "' /></td>";

            // Up-front purchase model - License (Onetime fee)
            if (title != "License:") {
                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td align='center'><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td align='center'><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select></td>";
            }
            else {
                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td align='center'><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td align='center'><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select>";
                row = row + "<div class='lbl-payment3'>Per Provider</div></td>";
            }

        }
        else if (controlCount == 3) { // If no quantity exists

            // title
            if ((rbselectedQuestionnaire == 3 || rbselectedQuestionnaire == 4) && tableId.indexOf("tableControlOnGoing") != -1 && title.indexOf("Other") != -1)
                row = "<tr><td style='width:27%; min-width:27%; max-width:27%'>" + img + "<span>" + title + "</span></td>";
            else if (rbselectedQuestionnaire == 4 && tableId.indexOf("tableControlOnGoing") == -1 && title.indexOf("Other") != -1)
                row = "<tr><td style='width:27%; min-width:27%; max-width:27%'>" + img + "<span>" + title + "</span></td>";
            else if (rbselectedQuestionnaire == 3 && tableId.indexOf("tableControlOnGoing") != -1 && title.indexOf("Interface") != -1)
                row = "<tr><td style='width:36%; min-width:36%; max-width:36%'>" + img + "<span>" + title + "</span></td>";
            else
                row = "<tr><td style='width:38.5%; min-width:38.5%; max-width:38.5%'>" + img + "<span>" + title + "</span></td>";

            if (title.indexOf("Interface") != -1)
                if (rbselectedQuestionnaire == 3 && tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td colspan='2' align='right' style='width:29%; min-width:29%; max-width:29%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnInterfaceType", type, title) + "</select></td>";
                else if (rbselectedQuestionnaire == 3 && tableId.indexOf("tableControlOneTime") == -1 && title.indexOf("Interface") != -1)
                    row = row + "<td colspan='2' align='right' style='width:27%; min-width:27%; max-width:27%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnInterfaceType", type, title) + "</select></td>";
                else if (rbselectedQuestionnaire == 3 && tableId.indexOf("tableControlOneTime") != -1 && title.indexOf("Interface") != -1)
                    row = row + "<td colspan='2' align='right' style='width:29%; min-width:29%; max-width:29%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnInterfaceType", type, title) + "</select></td>";
                else
                    row = row + "<td colspan='2' align='right' style='width:27%; min-width:27%; max-width:27%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnInterfaceType", type, title) + "</select></td>";

            else if (title.indexOf("Other") != -1)
                row = row + "<td colspan='2' align='right' style='width:38.3%; min-width:38.3%; max-width:38.3%'><span class='label-fee-title'>Type:</span><input id='" + genericControlId + countControl + "2' class='ddl-fee' type='text' value='" + type + "' /></td>";
            else
                row = row + "<td colspan='2' align='right' style='width:38.3%; min-width:38.3%; max-width:38.3%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnType", type, title) + "</select></td>";

            if (rbselectedQuestionnaire == 3 && tableId.indexOf("tableControlOnGoing") != -1 && title.indexOf("Interface") != -1)
                row = row + "<td align='center' style='width:11.5%; min-width:11.5%; max-width:11.5%'><input id='" + genericControlId + countControl + "3' class='txt-Amount' type='text' value='" + amount + "' /></td>";
            else
                row = row + "<td align='center' style='width:11%; min-width:11%; max-width:11%'><input id='" + genericControlId + countControl + "3' class='txt-Amount' type='text' value='" + amount + "' /></td>";

            // Up-front purchase model - License (Onetime fee)
            if (title != "License:") {
                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select></td>";
            }
            else {
                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select>";
                row = row + "<div class='lbl-payment2'>Per Provider</div></td>";

            }
        }
    }
    else {
        // other browsers firefox etc...        
        $(".selection :checked").each(function () {
            var selectedValue = $(this).val();
            if (parseInt(selectedValue) == 3 && tableId.indexOf("tableControlOneTime") != -1) {
                row = "<tr><td style='width:31%; min-width:31%; max-width:31%'>" + img + "<span>" + title + "</span></td>";
            }
            else {
                row = "<tr><td style='width:28.8%; min-width:28.8%; max-width:28.8%'>" + img + "<span>" + title + "</span></td>";
            }
        });


        if (controlCount == 4) { // if Qty Exists
            row = row + "<td style='width:15%; min-width:15%; max-width:15%'><span class='label-fee-title'>Qty:</span><input id='" + genericControlId + countControl + "1' class='txt-quantity' type='text' value='" + quantity + "' /></td>";
            if (title.indexOf("Interface") != -1)
                row = row + "<td style='width:32%; min-width:32%; max-width:32%'><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnInterfaceType", type, title) + "</select></td>";
            else if (title.indexOf("Other") != -1) {
                row = row + "<td style='width:32%; min-width:32%; max-width:32%'><span class='label-fee-title'>Type:</span><input id='" + genericControlId + countControl + "2' class='ddl-fee' type='text' value='" + type + "' /></td>";
            }
            else
                row = row + "<td style='width:32%; min-width:32%; max-width:32%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnType", type, title) + "</select></td>";

            row = row + "<td style='width:10%; min-width:10%; max-width:10%'><input id='" + genericControlId + countControl + "3' class='txt-Amount' type='text' value='" + amount + "' /></td>";

            // Up-front purchase model - License (Onetime fee)
            if (title != "License:") {
                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td ><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td ><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select></td>";
            }
            else {

                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td ><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td ><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select>";
                row = row + "<div class='lbl-payment2'>Per Provider</div></td>";
            }

        }
        else if (controlCount == 3) { // If Qty doesn't exists
            if (title.indexOf("Interface") != -1)
                row = row + "<td colspan='2' align='right' style='width:58%; min-width:58%; max-width:58%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnInterfaceType", type, title) + "</select></td>";
            else if (title.indexOf("Other") != -1) {
                row = row + "<td colspan='2' align='right' style='width:58%; min-width:58%; max-width:58%'><span class='label-fee-title'>Type:</span><input id='" + genericControlId + countControl + "2' class='ddl-fee' type='text' value='" + type + "' /></td>";
            }
            else
                row = row + "<td colspan='2' align='right' style='width:58%; min-width:58%; max-width:58%'><span class='label-fee-title'>Type:</span><select id='" + genericControlId + countControl + "2' class='ddl-fee'>" + ParseValues("hdnType", type, title) + "</select></td>";


            $(".selection :checked").each(function () {
                var selectedValue = $(this).val();
                if (parseInt(selectedValue) == 3 && tableId.indexOf("tableControlOneTime") != -1) {
                    row = row + "<td align='left' style='width:18%; min-width:18%; max-width:18%'><input id='" + genericControlId + countControl + "3' class='txt-Amount' type='text' value='" + amount + "' /></td>";
                }
                else {
                    row = row + "<td align='center' style='width:18%; min-width:18%; max-width:18%'><input id='" + genericControlId + countControl + "3' class='txt-Amount' type='text' value='" + amount + "' /></td>";
                }
            });

            // Up-front purchase model - License (Onetime fee)
            if (title != "License:") {
                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='ddl-fee'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select></td>";
            }
            else {
                if (tableId.indexOf("tableControlOnGoing") != -1)
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOngoingPaymentMethod", paymentMethod, title);
                else
                    row = row + "<td align='left'><select id='" + genericControlId + countControl + "4' class='hidden'>" + ParseValues("hdnOneTimePaymentMethod", paymentMethod, title);

                row = row + "</select>";
                row = row + "<div class='lbl-payment2'>Per Provider</div></td>";
            }
        }
    }

    countControl = countControl + 1;
    row = row + "</tr>";

    //$('#' + tableId + "").prepend(row);
    $('#' + tableId + "  tr:last").before(row);

    // enable validation
    EnableValidation();

}

function DisbaleSystems() {
    $("#btnAddEHR").attr("disabled", "disabled");
}

function EnabledSystems() {
    $("#btnAddEHR").removeAttr("disabled");
}