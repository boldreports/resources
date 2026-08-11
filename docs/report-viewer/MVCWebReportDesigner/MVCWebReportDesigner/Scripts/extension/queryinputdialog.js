var QueryInputDialog = (function () {
    function QueryInputDialog(designerCont) {
        this.divDialog = null;
        this.contentContainer = null;
        this.actionTypeDropDown = null;
        this.methodTextbox = null;
        this.rawTextArea = null;
        this.queryDesigner = null;
        this.queryInfo = null;
        this.callBackMethod = '';
        this.callBackInstance = null;
        this.designId = designerCont.attr('id');
        this.id = 'webapi_query_Container';
        this.renderQueryInputDialog();
    }
    QueryInputDialog.prototype.renderQueryInputDialog = function () {
        this.divDialog = ej.buildTag('div.e-reportdesigner-designer-configuration e-userselect', '', {
            'height': 'auto',
        }, {
            'title': this.getLocale('title'),
            'id': this.id + '_query_input_dialog'
        });
        var containerDiv = ej.buildTag('div.e-reportdesigner-scroller', '', {
            'height': '455px',
            'width': '380px'
        }, {
            'id': this.id + '_query_input_container_div'
        });
        var scrollerContent = ej.buildTag('div', '', {}, { 'id': this.id + '_query_input_scroll_container' });
        this.contentContainer = ej.buildTag('div', '', {
            'width': '100%',
            'height': '100%'
        }, { 'id': this.id + '_query_input_content_container' });
        scrollerContent.append(this.contentContainer);
        containerDiv.append(scrollerContent);
        this.divDialog.append(containerDiv);
        $(document.body).append(this.divDialog);
        this.renderWebDatasourcePanel(this.contentContainer);
        this.renderErrorToolTip(this.divDialog);
        this.divDialog.ejDialog({
            width: 'auto',
            height: 'auto',
            minWidth: 380,
            minHeight: 333,
            enableResize: false,
            showOnInit: false,
            enableModal: true,
            showFooter: true,
            cssClass: 'e-rptdesigner-dialog'
        });
        this.renderFooter();
        containerDiv.ejScroller({ height: '455px', buttonSize: 0, scrollerSize: 9, enableTouchScroll: true });
    };
    QueryInputDialog.prototype.renderFooter = function () {
        this.footerTag = $('#' + this.id + '_query_input_dialog_foot');
        var cancelButton = ej.buildTag('input.e-rptdesigner-cancelbtn', '', {
            'float': 'right'
        }, { 'id': this.id + '_query_input_btn_cancel' });
        var okButton = ej.buildTag('input.e-rptdesigner-okbtn', '', {
            'float': 'right'
        }, {
            'id': this.id + '_query_input_btn_ok'
        });
        this.footerTag.append(cancelButton).append(okButton);
        okButton.ejButton({
            text: this.getLocale('ok'),
            enabled: true,
            width: '60px',
            height: '23px',
            showRoundedCorner: false,
            size: 'mini',
            'click': $.proxy(this.updateQuery, this),
        });
        cancelButton.ejButton({
            width: '60px',
            text: this.getLocale('cancel'),
            height: '23px',
            showRoundedCorner: false,
            size: 'mini',
            'click': $.proxy(this.closeDialog, this, true)
        });
    };
    QueryInputDialog.prototype.renderWebDatasourcePanel = function (target) {
        var fields = { id: 'id', value: 'value', text: 'text' };
        this.renderDropDownItem(this.getLocale('Action'), this.id + '_query_input_action', target, this.getDropdownValues(), fields, this.onMethodChange, this);
        this.renderTextBoxItem(this.getLocale('Method'), this.id + '_query_input_method', false, target, 241);
        this.renderParameterFields(target, this.id + '_query_input_parameters', 'parameters', this.getLocale('Parameters'));
        this.renderTextArea(this.getLocale('Raw'), this.id + '_query_input_raw_txt', target, '');
        this.renderParameterFields(target, this.id + '_query_input_headers', 'headers', this.getLocale('Headers'));
        var action = this.contentContainer.find('#' + this.id + '_query_input_action');
        if (action.length > 0) {
            this.actionTypeDropDown = this.contentContainer.find('#' + this.id + '_query_input_action').data('ejDropDownList');
            this.actionTypeDropDown.selectItemsByIndices('0');
        }
        this.methodTextbox = this.contentContainer.find('#' + this.id + '_query_input_method');
        this.rawTextArea = this.contentContainer.find('#' + this.id + '_query_input_raw_txt');
        this.contentContainer.find('#' + this.id + '_query_input_raw_txt_container').hide();
        this.methodTextbox.bind('keyup', $.proxy(this.onTextBoxKeyup, this, this.methodTextbox.attr('id')));
    };
    QueryInputDialog.prototype.getDropdownValues = function () {
        var actionJson = [
            { id: 'get', value: 'GET', text: this.getLocale('GET') },
            { id: 'Post', value: 'POST', text: this.getLocale('POST') }
        ];
        return actionJson;
    };
    QueryInputDialog.prototype.renderDropDownItem = function (name, id, target, datasource, fields, fnction, context) {
        var configTableParent = ej.buildTag('div', '', {}, { 'id': id + '_container' });
        var configTable = ej.buildTag('table.e-designer-dsconfig-table', '', {
            'width': '100%',
            'padding-top': '10px',
            'margin-left': '0px'
        }, {
            'unselectable': 'on',
            'id': id + '_table'
        });
        var row = $('<tr id=' + id + '_tr/>');
        var coltxt = ej.buildTag('td', '', { width: '60%' }, { 'id': id + '_td' });
        var dropdown = ej.buildTag('input', '', {}, { 'id': id, 'value': '', 'spellcheck': 'false' });
        coltxt.append(dropdown);
        row.append(this.getCaptionLabel(name, id), coltxt);
        configTable.append(row);
        configTableParent.append(configTable);
        target.append(configTableParent);
        dropdown.ejDropDownList({
            width: '254px', dataSource: datasource, fields: fields, change: $.proxy(fnction, context),
            cssClass: 'e-designer-ejwidgets e-designer-content-label', showRoundedCorner: true
        });
    };
    QueryInputDialog.prototype.renderTextArea = function (name, id, target, value) {
        var configTableParent = ej.buildTag('div', '', {}, { 'id': id + '_container' });
        var configTable = ej.buildTag('table.e-designer-dsconfig-table', '', {
            'width': '100%',
            'padding-top': '10px',
            'margin-left': '0px'
        }, {
            'unselectable': 'on',
            'id': id + '_table'
        });
        var rowtxt = $('<tr></tr>');
        var coltxt = ej.buildTag('td', '', {}, { 'colspan': '3', 'id': id + '_td' });
        var txtbox = ej.buildTag('textarea.e-textarea e-ejinputtext e-designer-content-label e-designer-constr-textarea', value, {
            'height': '122px', 'width': '362px', 'resize': 'none', 'text-indent': '0px', 'overflow': 'hidden', 'padding': '5px'
        }, {
            'id': id, 'type': 'textarea', 'spellcheck': 'false'
        });
        coltxt.append(txtbox);
        rowtxt.append(coltxt);
        configTable.append(this.getRowCaption(name, id), rowtxt);
        configTableParent.append(configTable);
        target.append(configTableParent);
    };
    QueryInputDialog.prototype.renderTextBoxItem = function (name, id, isPasswd, target, width, value) {
        var configTableParent = ej.buildTag('div', '', {}, { 'id': id + '_container' });
        var configTable = ej.buildTag('table.e-designer-dsconfig-table', '', {
            'width': '100%',
            'padding-top': '10px',
            'margin-left': '0px'
        }, {
            'unselectable': 'on',
            'id': id + '_table'
        });
        var row = $('<tr id=' + id + '_tr' + '/>');
        var coltxt = ej.buildTag('td', '', { width: '60%' }, { 'id': id + '_td' });
        var txtbox = ej.buildTag('input.e-textbox e-designer-content-label', '', {
            'height': '24px', 'width': width + 'px'
        }, {
            'id': id,
            'type': isPasswd ? 'password' : 'text',
            'value': value,
            'spellcheck': 'false',
            'placeholder': this.getLocale('MethodName')
        });
        coltxt.append(txtbox);
        row.append(this.getCaptionLabel(name, id), coltxt);
        configTable.append(row);
        configTableParent.append(configTable);
        target.append(configTableParent);
    };
    QueryInputDialog.prototype.renderParameterFields = function (target, id, itemType, headerText) {
        var configTableParent = ej.buildTag('div', '', {}, { 'id': id + '_container' });
        var configTable = ej.buildTag('table.e-designer-dsconfig-table', '', {
            'width': '100%',
            'padding-top': '10px',
            'margin-left': '0px'
        }, {
            'unselectable': 'on',
            'id': id + '_table'
        });
        var basicRow = ej.buildTag('tr', '', {}, {});
        var basicCol = ej.buildTag('td', '', {}, {});
        var rootDiv = ej.buildTag('div', '', {
            'width': 'auto', 'height': '100%', 'display': 'block',
            'margin-top': '0px', 'overflow': 'auto'
        }, { 'id': id });
        var bodyDiv = ej.buildTag('div', '', {}, {});
        var bodyTable = ej.buildTag('table', '', {}, {});
        var newRow = ej.buildTag('tr', '', {}, {});
        var newCol = ej.buildTag('td', '', { 'width': '420px' }, {});
        var lblHdrText = ej.buildTag('Label.e-designer-title-label editLabel', headerText, { 'margin-left': '-3px' }, {
            'type': 'label',
            'id': id + '_labelText'
        });
        var spanDiv = ej.buildTag('div', '', { 'cursor': 'pointer', 'float': 'right' });
        var addSpan = ej.buildTag('span.e-chk-image e-icon e-plus e-rptdesigner-add-icon', '', {}, { 'id': this.id + '_' + itemType + '_add_span' });
        var addDiv = ej.buildTag('div', '', { 'cursor': 'pointer', 'float': 'right', 'padding-top': '2px' }, {});
        var textSpan = ej.buildTag('span.e-btntxt e-rptdesigner-add-btn', this.getLocale('Add'), {}, {});
        var containerDiv = ej.buildTag('div.e-reportdesigner-scroller e-designer-border-dialog', '', {
            'height': '126px',
            'padding': '5px 0px',
            'border': '1px solid #c9cbcc',
            'border-radius': '3px'
        }, {
            'id': this.id + '_' + itemType + '_val_parent'
        });
        var scrollDiv = ej.buildTag('div.e-items', '', {}, { 'id': this.id + '_' + itemType + '_container_div' });
        configTable.append(basicRow);
        configTableParent.append(configTable);
        target.append(configTableParent);
        basicRow.append(basicCol);
        basicCol.append(rootDiv);
        rootDiv.append(bodyDiv, containerDiv);
        bodyDiv.append(bodyTable);
        bodyTable.append(newRow);
        newRow.append(newCol);
        newCol.append(lblHdrText, addDiv, spanDiv);
        addDiv.append(textSpan);
        spanDiv.append(addSpan);
        containerDiv.append(scrollDiv);
        containerDiv.ejScroller({
            height: 126 + 'px',
            scrollerSize: 12,
            autoHide: false,
            enableTouchScroll: false
        });
        spanDiv.bind('click', $.proxy(this.renderSetValueFields, this, itemType));
        addDiv.bind('click', $.proxy(this.renderSetValueFields, this, itemType));
    };
    QueryInputDialog.prototype.renderSetValueFields = function (itemType) {
        var containerId = this.contentContainer.find('#' + this.id + '_' + itemType + '_container_div');
        var parentContainer = this.contentContainer.find('#' + this.id + '_' + itemType + '_val_parent');
        var className = 'e-reportdesigner-webParam-row';
        var rowId = ej.getGuid(this.id + '_row_');
        var divElement = ej.buildTag('div', '', {
            'padding': '4px'
        }, {
            'id': rowId,
            'rowId': rowId,
            'class': className
        });
        containerId.append(divElement);
        var containerTbl = ej.buildTag('table', '', {
            'width': '100%',
            'background': '#eeeeee',
            'border-radius': '2px',
            'box-sizing': 'border-box',
            '-moz-box-sizing': 'border-box',
            '-webkit-box-sizing': 'border-box'
        }, {});
        var container = ej.buildTag('tr', '', { 'height': '30px' }, {});
        var lblCol = ej.buildTag('td', '', { 'width': '114px' }, { 'id': rowId + '_' + itemType + '_lbl_txtCol' });
        var lblTxtBox = ej.buildTag('input.e-textbox e-designer-textbox-border e-designer-border e-designer-exclude e-rpt-textName', '', {
            'height': '20px',
            'width': '130px',
            'font-size': '11px'
        }, {
            'type': 'text',
            'id': rowId + '_' + itemType + '_lbl_txt',
            'placeholder': this.getLocale('Name'),
            'spellcheck': false
        });
        divElement.append(containerTbl);
        containerTbl.append(container);
        container.append(lblCol);
        lblCol.append(lblTxtBox);
        var deleteCol = ej.buildTag('td', '', { 'width': '25px', 'padding-right': '5px' }, {});
        var deleteBtn = ej.buildTag('span.e-chk-image e-icon e-cross-circle e-designer-delete-icon', '', {
            'cursor': 'default',
            'padding-top': '4px',
            'float': 'right',
            'color': '#d9534f',
            'font-size': '13px'
        }, {
            'id': rowId + '_' + itemType + '_del_icon',
            'title': this.getLocale('Close')
        });
        var textCol = ej.buildTag('td', '', { 'width': '114px', 'padding-left': '4px' }, {});
        var txtBox = ej.buildTag('input.e-textbox e-designer-textbox-border e-designer-border e-designer-exclude e-rpt-textValue', '', {
            'height': '20px',
            'width': '130px',
            'font-size': '11px'
        }, {
            'type': 'text',
            'id': rowId + '_' + itemType + '_value_txt',
            'placeholder': this.getLocale('Value'),
            'spellcheck': false
        });
        var errorCol = ej.buildTag('td', '', { 'padding-top': '1px', width: '25px' }, { 'id': rowId + '_webParam_error_td' });
        this.renderErrIndicator(errorCol, this.id + '_query_input_dialog');
        container.append(textCol);
        textCol.append(txtBox);
        container.append(errorCol);
        container.append(deleteCol);
        deleteCol.append(deleteBtn);
        deleteBtn.bind('click', $.proxy(this.deleteRow, this, divElement, parentContainer));
        var outerDivHeight = this.contentContainer.find('#' + this.id + '_' + itemType + '_container_div').height();
        if (parentContainer.height() > outerDivHeight) {
            parentContainer.find('.e-content').removeClass('e-content');
        }
        else {
            this.scrollerRefresh(parentContainer);
        }
        return rowId;
    };
    QueryInputDialog.prototype.getCaptionLabel = function (caption, id) {
        var labelCell = ej.buildTag('td', '', { width: '40%' });
        var label = ej.buildTag('label.e-designer-title-label editLabel', caption, { 'max-width': '200px' }, {
            'id': id + '_labelText'
        });
        labelCell.append(label);
        return labelCell;
    };
    QueryInputDialog.prototype.getInfoIndicator = function (id) {
        var infoCell = ej.buildTag('td', '', {}, { 'id': id + '_info_icon_td', 'width': '20px' });
        this.renderInfoIndicator(infoCell, this.id);
        return infoCell;
    };
    QueryInputDialog.prototype.getRowCaption = function (caption, id) {
        var row = ej.buildTag('tr', '', {});
        var labelCell = ej.buildTag('td', '', {});
        var label = ej.buildTag('label.e-designer-title-label editLabel', caption, { 'max-width': '200px' }, {
            'id': id + '_labelText'
        });
        labelCell.append(label);
        row.append(labelCell);
        var infoLabelCell = ej.buildTag('td', '', {});
        var infoLabel = ej.buildTag('label.e-designer-title-label editLabel e-rptdesigner-webapi-raw', this.getLocale('jsontext'), { 'font-size': '11px', 'float': 'right', 'padding-bottom': '3px' }, {
            'id': id + '_info_labelText'
        });
        infoLabelCell.append(infoLabel);
        row.append(infoLabelCell);
        var infoCell = this.getInfoIndicator(id);
        row.append(infoCell);
        return row;
    };
    QueryInputDialog.prototype.populateParameterVal = function (parameters, itemType) {
        if (!ej.isNullOrUndefined(parameters) && parameters.length > 0) {
            for (var index = 0; index < parameters.length; index++) {
                var parameter = parameters[index];
                var rowId = this.renderSetValueFields(itemType);
                var nameTag = $('#' + rowId + '_' + itemType + '_lbl_txt');
                var valTag = $('#' + rowId + '_' + itemType + '_value_txt');
                nameTag.removeClass('e-rptdesigner-txtmark').val(parameter.Key);
                valTag.removeClass('e-rptdesigner-txtmark').val(parameter.Value);
            }
        }
    };
    QueryInputDialog.prototype.deleteRow = function (rowInfo, parentContainer) {
        rowInfo.remove();
        this.scrollerRefresh(parentContainer);
    };
    QueryInputDialog.prototype.onMethodChange = function (args) {
        if (args.selectedValue === 'GET') {
            this.contentContainer.find('#' + this.id + '_query_input_parameters_container').show();
            this.contentContainer.find('#' + this.id + '_query_input_raw_txt_container').hide();
        }
        else {
            this.contentContainer.find('#' + this.id + '_query_input_parameters_container').hide();
            this.contentContainer.find('#' + this.id + '_query_input_raw_txt_container').show();
        }
    };
    QueryInputDialog.prototype.openQueryInputDialog = function (queryInfo, instance, method) {
        this.resetQueryInputDialog();
        this.queryInfo = queryInfo;
        this.callBackInstance = instance;
        this.callBackMethod = method;
        $('#' + this.id + '_query_input_dialog').data('ejDialog').open();
        this.populateQueryInfo();
    };
    QueryInputDialog.prototype.resetQueryInputDialog = function () {
        this.queryInfo = null;
        this.callBackInstance = null;
        this.callBackMethod = null;
        this.hideValidationMsg(this.methodTextbox.attr('id'));
        this.actionTypeDropDown.setModel({ 'selectedIndex': 0 });
        this.methodTextbox.val('');
        this.rawTextArea.val('');
        var parameterContainer = $('#' + this.id + '_parameters_container_div');
        var headerContainer = $('#' + this.id + '_headers_container_div');
        parameterContainer.empty();
        headerContainer.empty();
        this.contentContainer.find('#' + this.id + '_query_input_parameters_container').show();
        this.contentContainer.find('#' + this.id + '_query_input_raw_txt_container').hide();
        this.scrollerRefresh(parameterContainer);
        this.scrollerRefresh(headerContainer);
    };
    QueryInputDialog.prototype.populateQueryInfo = function () {
        if (!ej.isNullOrUndefined(this.queryInfo) && this.queryInfo.length > 0) {
            var headers = [];
            var parameters = [];
            for (var index = 0; index < this.queryInfo.length; index++) {
                var queryData = this.queryInfo[index];
                if (queryData.Key.indexOf('ActionType') !== -1) {
                    this.actionTypeDropDown.selectItemByValue(queryData.Value);
                }
                if (queryData.Key.indexOf('Method') !== -1) {
                    this.methodTextbox.val(queryData.Value);
                }
                if (queryData.Key.indexOf('RawData') !== -1) {
                    this.rawTextArea.val(queryData.Value);
                }
                if (queryData.Key.indexOf('Headers') !== -1) {
                    for (var headerIndex = 0; headerIndex < queryData.Value.length; headerIndex++) {
                        var headerData = queryData.Value[headerIndex];
                        headers.push({ Key: headerData.Key, Value: headerData.Value });
                    }
                }
                if (queryData.Key.indexOf('Parameters') !== -1) {
                    for (var parameterIndex = 0; parameterIndex < queryData.Value.length; parameterIndex++) {
                        var parameterData = queryData.Value[parameterIndex];
                        parameters.push({ Key: parameterData.Key, Value: parameterData.Value });
                    }
                }
            }
            if (headers.length > 0) {
                this.populateParameterVal(headers, 'headers');
            }
            if (parameters.length > 0) {
                this.populateParameterVal(parameters, 'parameters');
            }
        }
    };
    QueryInputDialog.prototype.updateQuery = function () {
        this.queryInfo = this.getQueryDataInfo();
        if (!ej.isNullOrUndefined(this.queryInfo)) {
            ej.ReportUtil.invokeMethod(this.callBackInstance, this.callBackMethod, [this.queryInfo]);
            this.closeDialog();
        }
    };
    QueryInputDialog.prototype.getQueryDataInfo = function () {
        var isValidQuery = true;
        var actionType = this.actionTypeDropDown.getSelectedValue();
        var methodName = this.methodTextbox.val().trim();
        var rawData = this.rawTextArea.val().trim();
        var parameters = this.getParameters('parameters');
        var headers = this.getParameters('headers');
        if (methodName.length === 0) {
            this.showValidationMsg(this.methodTextbox.attr('id'), true);
            isValidQuery = false;
        }
        else {
            this.hideValidationMsg(this.methodTextbox.attr('id'));
        }
        if (isValidQuery && !parameters.hasErrors && !headers.hasErrors) {
            var queryInfo = [];
            var headerValues = this.getHeaderValues(headers.parametersVal);
            var parameterValues = this.getParameterValues(parameters.parametersVal);
            queryInfo.push({ Key: 'ActionType', Value: actionType });
            if (methodName.length > 0) {
                queryInfo.push({ Key: 'Method', Value: methodName });
            }
            if (actionType.toLowerCase() === 'post') {
                if (rawData.length === 0) {
                    queryInfo.push({ Key: 'RawData', Value: '""' });
                }
                else {
                    queryInfo.push({ Key: 'RawData', Value: rawData });
                }
            }
            if (headerValues.length > 0) {
                queryInfo.push({ Key: 'Headers', Value: headerValues });
            }
            if (parameterValues.length > 0 && actionType.toLowerCase() === 'get') {
                queryInfo.push({ Key: 'Parameters', Value: parameterValues });
            }
            return queryInfo;
        }
        return null;
    };
    QueryInputDialog.prototype.getHeaderValues = function (headers) {
        var headerValues = [];
        for (var index = 0; index < headers.length; index++) {
            headerValues.push({
                Key: headers[index].Key,
                Value: headers[index].Value
            });
        }
        return headerValues;
    };
    QueryInputDialog.prototype.getParameterValues = function (parameters) {
        var parameterValues = [];
        for (var index = 0; index < parameters.length; index++) {
            parameterValues.push({
                Key: parameters[index].Key,
                Value: parameters[index].Value.trim()
            });
        }
        return parameterValues;
    };
    QueryInputDialog.prototype.getParameters = function (itemType) {
        var _this = this;
        var parameters = [];
        var errorContent = '';
        var hasErrors = false;
        var availableRows = this.contentContainer.find('#' + this.id + '_' + itemType + '_container_div .e-reportdesigner-webParam-row');
        availableRows.each(function (index, obj) {
            var rowId = $(obj).attr('id');
            var nameText = _this.getValues($('#' + rowId + '_' + itemType + '_lbl_txt'));
            var valueText = _this.getValues($('#' + rowId + '_' + itemType + '_value_txt'));
            errorContent += _this.validateValues(nameText, errorContent, $('#' + rowId + '_' + itemType + '_lbl_txt'));
            errorContent += _this.validateValues(valueText, errorContent, $('#' + rowId + '_' + itemType + '_value_txt'));
            if (!ej.isNullOrUndefined(errorContent) && errorContent.length !== 0) {
                _this.showErrIndictor($('#' + rowId + '_webParam_error_td'), true, errorContent);
                errorContent = '';
                hasErrors = true;
            }
            else {
                parameters.push({ Key: nameText, Value: valueText });
                _this.showErrIndictor($('#' + rowId + '_webParam_error_td'), false, errorContent);
            }
        });
        if (!hasErrors) {
            return { hasErrors: false, parametersVal: parameters };
        }
        return { hasErrors: true, parametersVal: parameters };
    };
    QueryInputDialog.prototype.getValues = function (targetTag) {
        var targetTagTxt = targetTag.val();
        var value;
        if (targetTag.hasClass('e-rptdesigner-txtmark')) {
            value = '';
        }
        else {
            value = targetTagTxt;
        }
        return value;
    };
    QueryInputDialog.prototype.validateValues = function (value, errorContent, id) {
        var content = '';
        if (((!ej.isNullOrUndefined(value) && value.length === 0) || ej.isNullOrUndefined(value))) {
            content += this.getLocale('specifyFields');
            this.updateHighlighter(id, true);
        }
        else {
            this.updateHighlighter(id, false);
        }
        if (this.contains(errorContent, content, true)) {
            content = '';
        }
        return content;
    };
    QueryInputDialog.prototype.updateHighlighter = function (target, isUpdate) {
        isUpdate ? target.addClass('e-rptdesigner-error').addClass('e-rptdesigner-error-radius') :
            target.removeClass('e-rptdesigner-error').removeClass('e-rptdesigner-error-radius');
    };
    QueryInputDialog.prototype.contains = function (actual, expected, ignoreCase) {
        if (ignoreCase) {
            return !this.isNull(actual)
                && !this.isNull(expected)
                && this.toLowerCase(actual).indexOf(this.toLowerCase(expected)) !== -1;
        }
        return !this.isNull(actual)
            && !this.isNull(expected)
            && actual.toString().indexOf(expected) !== -1;
    };
    QueryInputDialog.prototype.isNull = function (actual) {
        return actual === null;
    };
    QueryInputDialog.prototype.toLowerCase = function (val) {
        return val ? val.toLowerCase ?
            val.toLowerCase() :
            val.toString().toLowerCase() :
            (val === 0 || val === false) ? val.toString() : '';
    };
    QueryInputDialog.prototype.renderErrIndicator = function (target, tooltipId, errMsg) {
        var errorIcon = ej.buildTag('span.e-rptdesigner-error-icon e-rptdesigner-errorinfo e-error-tip', '', {
            'float': 'right',
            'display': 'none',
            'padding-right': '2px'
        }, {
            'e-errormsg': errMsg,
            'e-tooltipId': tooltipId
        });
        target.append(errorIcon);
    };
    QueryInputDialog.prototype.renderInfoIndicator = function (target, ctrlId, errMsg) {
        var infoIcon = ej.buildTag('span.e-toolbarfonticonbasic e-qrydesigner-webapi-rawdata', '', {
            'float': 'right', 'display': 'block',
            'padding': '2px', 'font-size': '12px',
            'border': '1px solid #ababab', 'border-radius': '3px'
        }, {});
        target.append(infoIcon);
        infoIcon.ejTooltip({
            position: {
                target: { horizontal: 'right', vertical: 'middle' },
                stem: { horizontal: 'left', vertical: 'top' }
            },
            animation: {
                effect: 'Fade',
                speed: 500
            },
            isBalloon: true,
            showShadow: true,
            showRoundedCorner: true,
            content: this.getTooltipTemplate(),
        });
        infoIcon.bind('mousedown touchstart', $.proxy(this.appendRawData, this, ctrlId));
    };
    QueryInputDialog.prototype.getTooltipTemplate = function () {
        var toolTipContent = '<div>' +
            '<div style = \'font-size: 12px; font-weight: 600; display: inline-block\'>String Type: </br></div>' +
            '<div style = \'font-size: 11px; display: inline-block\'>"ParameterValue/@ParamValue"</div>' +
            '</div>' +
            '<div>' +
            '<div style = \'font-size: 12px; font-weight: 600; display: inline-block\'>Numeric Type: </br></div>' +
            '<div style = \'font-size: 11px; display: inline-block\'>485</div>' +
            '</div>' +
            '<div>' +
            '<div style = \'font-size: 12px; font-weight: 600\'>Object Type:</div>' +
            '<div style = \'font-size: 11px\'>{ "Name": "Stephen", "Designation": "Manager" }</div>' +
            '</div>' +
            '<div>' +
            '<div style = \'font-size: 12px; font-weight: 600\'>Complex Object Type:</div>' +
            '<div style = \'font-size: 11px;\'>{ "Items": { "Quantity": "10 Units", "Price": "20 Rs" }, "Characteristics": { "color": "blue", "weight": "2 lb" } }</div>' +
            '</div>';
        return toolTipContent;
    };
    QueryInputDialog.prototype.showErrIndictor = function (target, isEnable, errMsg) {
        var errorIcon = target.find('.e-error-tip');
        errorIcon.css('display', isEnable ? 'block' : 'none');
        if (errMsg) {
            errorIcon.attr('e-errormsg', errMsg);
        }
        if (isEnable) {
            var tooltipId = errorIcon.attr('e-tooltipId');
            var ejTooltip = $('#' + tooltipId).data('ejTooltip');
            ejTooltip.setModel({
                target: '.e-rptdesigner-error-icon',
            });
        }
    };
    QueryInputDialog.prototype.renderErrorToolTip = function (target) {
        if (target && target.length !== 0 && !target.data('ejTooltip')) {
            target.ejTooltip({
                target: '.e-designer-tooltip',
                position: {
                    target: { horizontal: 'bottom', vertical: 'bottom' },
                    stem: { horizontal: 'right', vertical: 'top' }
                },
                tip: {
                    adjust: {
                        xValue: 10,
                        yValue: 100
                    }
                },
                animation: {
                    effect: 'Fade',
                    speed: 500
                },
                isBalloon: false,
                showShadow: true,
                showRoundedCorner: true,
                content: 'Exception Message is not configured',
                beforeOpen: $.proxy(this.beforeOpenTooltip, this)
            });
        }
    };
    QueryInputDialog.prototype.beforeOpenTooltip = function (args) {
        if (args.event && args.event.target) {
            args.cancel = !ej.isNullOrUndefined(args.event.buttons) && args.event.buttons !== 0;
            var target = args.event.target;
            if (target) {
                var tooltipId = $(target).attr('e-tooltipId');
                var errMsg = $(target).attr('e-errormsg');
                var ejTooltip = $('#' + tooltipId).data('ejTooltip');
                ejTooltip.setModel({
                    content: errMsg ? errMsg : ''
                });
            }
        }
    };
    QueryInputDialog.prototype.appendRawData = function (ctrlId, args) {
        this.rawTextArea.val('{ "Items": { "Quantity": "10 Units", "Price": "20 Rs" }, "Characteristics": { "color": "blue", "weight": "2 lb" }}');
    };
    QueryInputDialog.prototype.showValidationMsg = function (id, isShow) {
        var errContainer = $('#' + id + '_td').find('.e-designer-content-label');
        if (isShow) {
            errContainer.addClass('e-rptdesigner-error');
        }
        else {
            errContainer.removeClass('e-rptdesigner-error');
        }
    };
    QueryInputDialog.prototype.hideValidationMsg = function (id) {
        this.showValidationMsg(id, false);
    };
    QueryInputDialog.prototype.onTextBoxKeyup = function (id, args) {
        this.hideValidationMsg(id);
    };
    QueryInputDialog.prototype.scrollerRefresh = function (target) {
        if (target.length > 0 && !ej.isNullOrUndefined(target.data('ejScroller'))) {
            target.data('ejScroller').refresh();
        }
    };
    QueryInputDialog.prototype.getLocale = function (text) {
        var queryDialogLocale;
        var defaultLocale = QueryInputDialog.Locale['en-US'];
        if (!ej.isNullOrUndefined(QueryInputDialog.Locale[this.locale])) {
            queryDialogLocale = QueryInputDialog.Locale[this.locale];
        }
        switch (text.toLowerCase()) {
            case 'title':
                if (queryDialogLocale && queryDialogLocale.titleText) {
                    return queryDialogLocale.titleText;
                }
                return defaultLocale.titleText;
            case 'ok':
                if (queryDialogLocale && queryDialogLocale.okButtonText) {
                    return queryDialogLocale.okButtonText;
                }
                return defaultLocale.okButtonText;
            case 'cancel':
                if (queryDialogLocale && queryDialogLocale.cancelButtonText) {
                    return queryDialogLocale.cancelButtonText;
                }
                return defaultLocale.cancelButtonText;
            case 'get':
                if (queryDialogLocale && queryDialogLocale.getText) {
                    return queryDialogLocale.getText;
                }
                return defaultLocale.getText;
            case 'post':
                if (queryDialogLocale && queryDialogLocale.postText) {
                    return queryDialogLocale.postText;
                }
                return defaultLocale.postText;
            case 'action':
                if (queryDialogLocale && queryDialogLocale.actionText) {
                    return queryDialogLocale.actionText;
                }
                return defaultLocale.actionText;
            case 'method':
                if (queryDialogLocale && queryDialogLocale.methodText) {
                    return queryDialogLocale.methodText;
                }
                return defaultLocale.methodText;
            case 'raw':
                if (queryDialogLocale && queryDialogLocale.rawText) {
                    return queryDialogLocale.rawText;
                }
                return defaultLocale.rawText;
            case 'jsontext':
                if (queryDialogLocale && queryDialogLocale.jsonText) {
                    return queryDialogLocale.jsonText;
                }
                return defaultLocale.jsonText;
            case 'headers':
                if (queryDialogLocale && queryDialogLocale.headersText) {
                    return queryDialogLocale.headersText;
                }
                return defaultLocale.headersText;
            case 'parameters':
                if (queryDialogLocale && queryDialogLocale.parametersText) {
                    return queryDialogLocale.parametersText;
                }
                return defaultLocale.parametersText;
            case 'methodname':
                if (queryDialogLocale && queryDialogLocale.methodNameText) {
                    return queryDialogLocale.methodNameText;
                }
                return defaultLocale.methodNameText;
            case 'add':
                if (queryDialogLocale && queryDialogLocale.addbuttonText) {
                    return queryDialogLocale.addbuttonText;
                }
                return defaultLocale.addbuttonText;
            case 'close':
                if (queryDialogLocale && queryDialogLocale.closeButtonToolTip) {
                    return queryDialogLocale.closeButtonToolTip;
                }
                return defaultLocale.closeButtonToolTip;
            case 'name':
                if (queryDialogLocale && queryDialogLocale.nameText) {
                    return queryDialogLocale.nameText;
                }
                return defaultLocale.nameText;
            case 'value':
                if (queryDialogLocale && queryDialogLocale.valueText) {
                    return queryDialogLocale.valueText;
                }
                return defaultLocale.valueText;
            case 'specifyfields':
                if (queryDialogLocale && queryDialogLocale.specifyFields) {
                    return queryDialogLocale.specifyFields;
                }
                return defaultLocale.specifyFields;
        }
        return text;
    };
    QueryInputDialog.prototype.updateCulture = function (culture) {
        var _this = this;
        this.locale = culture;
        var actionLabel = this.contentContainer.find('#' + this.id + '_query_input_action_labelText');
        var methodLabel = this.contentContainer.find('#' + this.id + '_query_input_method_labelText');
        var parametersLabel = this.contentContainer.find('#' + this.id + '_query_input_parameters_labelText');
        var headersLabel = this.contentContainer.find('#' + this.id + '_query_input_headers_labelText');
        var rawLabel = this.contentContainer.find('#' + this.id + '_query_input_raw_txt_labelText');
        var methodTextBox = this.contentContainer.find('#' + this.id + '_query_input_method');
        var jsonLabel = this.contentContainer.find('#' + this.id + '_info_labelText');
        if (!ej.isNullOrUndefined(actionLabel) && actionLabel.length > 0) {
            actionLabel.text(this.getLocale('Action'));
        }
        if (!ej.isNullOrUndefined(methodLabel) && methodLabel.length > 0) {
            methodLabel.text(this.getLocale('Method'));
        }
        if (!ej.isNullOrUndefined(methodTextBox) && methodTextBox.length > 0) {
            methodTextBox.attr('placeholder', this.getLocale('methodname'));
        }
        if (!ej.isNullOrUndefined(parametersLabel) && parametersLabel.length > 0) {
            parametersLabel.text(this.getLocale('Parameters'));
        }
        if (!ej.isNullOrUndefined(headersLabel) && headersLabel.length > 0) {
            headersLabel.text(this.getLocale('Headers'));
        }
        if (!ej.isNullOrUndefined(rawLabel) && rawLabel.length > 0) {
            rawLabel.text(this.getLocale('Raw'));
        }
        if (!ej.isNullOrUndefined(jsonLabel) && jsonLabel.length > 0) {
            jsonLabel.text(this.getLocale('jsontext'));
        }
        this.contentContainer.find('.e-rptdesigner-add-btn').each(function (index, element) {
            $(element).text(_this.getLocale('Add'));
        });
        this.contentContainer.find('.e-rpt-textName').each(function (index, element) {
            $(element).attr('placeholder', _this.getLocale('name'));
        });
        this.contentContainer.find('.e-rpt-textValue').each(function (index, element) {
            $(element).attr('placeholder', _this.getLocale('value'));
        });
        this.contentContainer.find('.e-designer-delete-icon').each(function (index, element) {
            $(element).attr('title', _this.getLocale('Close'));
        });
        this.contentContainer.find('.e-reportdesigner-webParam-row').each(function (index, element) {
            $(element).find('.e-error-tip').attr('e-errormsg', _this.getLocale('specifyFields'));
        });
        this.divDialog.data('ejDialog').setModel({ 'title': this.getLocale('title') });
        this.footerTag.find('#' + this.id + '_query_input_btn_ok').data('ejButton').setModel({ text: this.getLocale('ok') });
        this.footerTag.find('#' + this.id + '_query_input_btn_cancel').data('ejButton').setModel({ text: this.getLocale('cancel') });
        var selectedValue = this.actionTypeDropDown.getSelectedValue();
        this.actionTypeDropDown.setModel({ 'dataSource': this.getDropdownValues() });
        this.actionTypeDropDown.selectItemByValue(selectedValue);
    };
    QueryInputDialog.prototype.closeDialog = function () {
        $('#' + this.id + '_query_input_dialog').data('ejDialog').close();
    };
    QueryInputDialog.prototype.dispose = function () {
        var queryDialog = $('#' + this.id + '_query_input_dialog');
        if (!ej.isNullOrUndefined(queryDialog) && queryDialog.length > 0 && !ej.isNullOrUndefined(queryDialog.data('ejDialog'))) {
            this.destroyEjObjects(queryDialog);
            this.destroyEjObjects(queryDialog, true);
            $('#' + this.id + '_query_input_dialog').remove();
        }
    };
    QueryInputDialog.prototype.destroyEjObjects = function (ejObjects, isRootEle) {
        var elements = isRootEle ? $(ejObjects) : $(ejObjects).find('.e-js');
        for (var i = 0; i < elements.length; i++) {
            var data = elements.eq(i).data();
            var wds = data['ejWidgets'];
            if (wds && wds.length) {
                for (var j = wds.length - 1; j >= 0; j--) {
                    if (data[wds[j]] && data[wds[j]].destroy) {
                        data[wds[j]].destroy();
                    }
                }
            }
        }
    };
    QueryInputDialog.prototype.enableButton = function (args) {
        var isWebApiDS = !ej.isNullOrUndefined(args.dataSource) && !ej.isNullOrUndefined(args.dataSource.ConnectionProperties)
            && args.dataSource.ConnectionProperties.DataProvider === 'WebAPI' ? args.isQueryMode : false;
        $('#' + this.id + '_query_webapi').parent().css('display', isWebApiDS ? 'inline-block' : 'none');
        $('#' + this.id + '_query_webapi').css('display', isWebApiDS ? 'inline-block' : 'none');
        $('#' + this.id + '_query_webapi_btn').css('display', isWebApiDS ? 'inline-block' : 'none');
        if (isWebApiDS) {
            $('#' + this.designId + '_div_designerTool .e-qrydesigner-switcher').removeClass('e-qrydesigner-separator');
        }
        else {
            $('#' + this.designId + '_div_designerTool .e-qrydesigner-switcher').addClass('e-qrydesigner-separator');
        }
        if (!$('#' + this.designId + '_design_switcher').is(':visible')) {
            $('#' + this.designId + '_div_designerTool .e-qrydesigner-switcher').addClass('e-qrydesigner-separator');
        }
    };
    QueryInputDialog.prototype.renderToolbarItems = function (args) {
        var $webApiUlTag = ej.buildTag('ul.e-rptdesigner-toolbarul', '', { 'border-right': '0 none', 'display': 'none' }, {});
        var $liTag = ej.buildTag('li.e-rptdesigner-toolbarli', '', { 'margin': '0px 1.5px', 'display': 'none' }, {
            'id': this.id + '_query_webapi'
        });
        var $span = ej.buildTag('span.e-rptdesigner-toolbar-icon e-toolbarfonticonbasic e-qrydesigner-webapi', '', {
            'color': 'black', 'font-size': '24px',
            'margin-left': '2px', 'margin-top': '2.5px', 'display': 'none'
        }, {
            'id': this.id + '_query_webapi_btn'
        });
        $liTag.append($span);
        $webApiUlTag.append($liTag);
        $(args.target).find('#' + this.designId + '_design_switcher').parent().after($webApiUlTag);
        $span.bind('mousedown touchstart', $.proxy(this.openDialog, this));
    };
    QueryInputDialog.prototype.openDialog = function (event) {
        var reportDesigner = $('#' + this.designId).data('boldReportDesigner');
        this.queryDesigner = reportDesigner.getInstance('QueryDesigner');
        this.openQueryInputDialog(this.getModel(this.queryDesigner.getQueryText()), this, 'updateQueryString');
    };
    QueryInputDialog.prototype.getModel = function (queryString) {
        var queryInfo = [];
        if (!ej.isNullOrUndefined(queryString) && queryString.trim().length > 0
            && queryString.indexOf(';') > 0) {
            var webApiQuery = queryString.split(';');
            for (var index = 0; index < webApiQuery.length; index++) {
                if (webApiQuery[index].indexOf('=') > 0) {
                    var query = webApiQuery[index].split('=');
                    var value = query[1];
                    if (query[0] === 'Parameters' || query[0] === 'Headers' || (query[0] === 'RawData' && query[1] === '\"\"')) {
                        value = JSON.parse(query[1]);
                    }
                    queryInfo.push({ Key: query[0], Value: value });
                }
            }
        }
        return queryInfo;
    };
    QueryInputDialog.prototype.updateQueryString = function (queryInfo) {
        if (!ej.isNullOrUndefined(queryInfo) && queryInfo.length > 0) {
            this.queryDesigner.setQueryText(this.getQueryString(queryInfo));
            this.queryDesigner.executeQuery();
        }
    };
    QueryInputDialog.prototype.getQueryString = function (queryInfo) {
        var query = '';
        for (var index = 0; index < queryInfo.length; index++) {
            var value = queryInfo[index].Value;
            if (typeof queryInfo[index].Value !== 'string') {
                value = JSON.stringify(queryInfo[index].Value);
            }
            query = query + queryInfo[index].Key + '=' + value;
            if (index < (queryInfo.length - 1)) {
                query = query + ';';
            }
        }
        return query;
    };
    return QueryInputDialog;
}());
QueryInputDialog.Locale = {};
QueryInputDialog.Locale['en-US'] = {
    titleText: 'Web API Query',
    okButtonText: 'Ok',
    cancelButtonText: 'Cancel',
    getText: 'GET',
    postText: 'POST',
    actionText: 'Action',
    methodText: 'Method*',
    rawText: 'RawData*',
    jsonText: 'Insert JSON String',
    headersText: 'Headers',
    parametersText: 'Parameters',
    methodNameText: 'Method name',
    addbuttonText: 'Add',
    nameText: 'Name',
    valueText: 'Value',
    closeButtonToolTip: 'Close',
    specifyFields: 'Specify the input fields'
};
QueryInputDialog.Locale['fr-FR'] = {
    titleText: 'Requête d\'API Web',
    okButtonText: 'D\'accord',
    cancelButtonText: 'Annuler',
    getText: 'OBTENIR',
    postText: 'POSTER',
    actionText: 'action',
    methodText: 'Méthode*',
    rawText: 'Données brutes*',
    jsonText: 'Insérer une chaîne JSON',
    headersText: 'Les entêtes',
    parametersText: 'Paramètres',
    methodNameText: 'Nom de la méthode',
    addbuttonText: 'Ajouter',
    nameText: 'prénom',
    valueText: 'Valeur',
    closeButtonToolTip: 'Fermer',
    specifyFields: 'Spécifiez les champs de saisie'
};
