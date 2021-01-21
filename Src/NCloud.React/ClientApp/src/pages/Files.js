"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_redux_1 = require("react-redux");
var FileStore = require("../store/Files");
var FileIcons = require("./img/filetypes");
var core_1 = require("@blueprintjs/core");
var Files = /** @class */ (function (_super) {
    __extends(Files, _super);
    function Files() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    // This method is called when the component is first added to the document
    Files.prototype.componentDidMount = function () {
        this.ensureDataFetched();
    };
    // This method is called when the route parameters change
    Files.prototype.componentDidUpdate = function () {
        this.ensureDataFetched();
    };
    Files.prototype.render = function () {
        return (React.createElement(React.Fragment, null,
            React.createElement(core_1.Alert, { confirmButtonText: "\u597D\u7684", isOpen: this.props.error, onClose: this.props.closeError },
                React.createElement("p", null, this.props.errorMessage)),
            React.createElement(core_1.Card, { elevation: 0, style: { width: "100%" } },
                React.createElement(core_1.Breadcrumbs, { collapseFrom: core_1.Boundary.START, items: this.props.breadItems })),
            this.renderFilesTable()));
    };
    Files.prototype.goBack = function () {
        this.props.history.goBack();
    };
    Files.prototype.redirect = function (baseId, id) {
        this.props.history.push("/files/" + baseId + "/" + id);
    };
    Files.prototype.onFileItemClicked = function (item) {
        if (item.type == FileStore.FileType.Directory) {
            this.redirect(item.baseId, item.id);
        }
        else {
        }
    };
    Files.prototype.ensureDataFetched = function () {
        if (!this.props.loading) {
            var baseId = this.props.match.params.baseId || this.props.baseId;
            var id = this.props.match.params.id || this.props.id;
            console.log("baseId:", baseId);
            console.log("id:", id);
            this.props.load(baseId, id);
        }
    };
    Files.prototype.getFileSize = function (size) {
        if (size > 0) {
            var sizeName = ["b", "KB", "MB", "GB", "TB"];
            var num = 0;
            while (size > 1024 && num < 5) {
                size = size / 1024;
                num++;
            }
            if (num < 1) {
                num = 1;
                size = 1;
            }
            return Math.round(size * 100) / 100 + " " + sizeName[num];
        }
        else {
            return "";
        }
    };
    Files.prototype.renderFileImage = function (type, isDirectory) {
        var icon = FileIcons.file;
        if (isDirectory) {
            icon = FileIcons.folder;
        }
        else {
            if (type === 'mp4') {
                icon = FileIcons.mp4;
            }
            else if (type === 'avi') {
                icon = FileIcons.avi;
            }
            else if (type === 'exe') {
                icon = FileIcons.exe;
            }
            else if (type === 'png' || type === 'jpg' || type === 'svg') {
                icon = FileIcons.jpg;
            }
            else if (type === 'html' || type === 'htm') {
                icon = FileIcons.html;
            }
            else if (type === 'pdf') {
                icon = FileIcons.pdf;
            }
            else if (type === 'doc' || type === 'docx') {
                icon = FileIcons.doc;
            }
            else if (type === 'ppt' || type === 'pptx') {
                icon = FileIcons.ppt;
            }
            else if (type === 'xls' || type === 'xlsx') {
                icon = FileIcons.xls;
            }
            else if (type === 'mp3') {
                icon = FileIcons.mp3;
            }
            else if (type === 'json') {
                icon = FileIcons.json;
            }
            else if (type === 'txt') {
                icon = FileIcons.txt;
            }
            else if (type === 'zip' || type === 'gz' || type === 'rar' || type === 'tar') {
                icon = FileIcons.zip;
            }
        }
        return React.createElement("img", { src: icon, className: "fileicon" });
    };
    Files.prototype.renderFilesTable = function () {
        var _this = this;
        if (this.props.loading) {
            console.log('loading data');
            return React.createElement(core_1.Spinner, { intent: core_1.Intent.NONE, size: core_1.Spinner.SIZE_STANDARD });
        }
        else {
            return (React.createElement(React.Fragment, null, this.props.children.map(function (file) {
                return React.createElement(core_1.Card, { interactive: true, elevation: core_1.Elevation.TWO, key: file.id },
                    React.createElement("div", { className: "grid", onClick: function () { return _this.onFileItemClicked(file); } },
                        React.createElement("div", { className: "grid-cell u3" }, _this.renderFileImage(file.ext, file.type == FileStore.FileType.Directory)),
                        React.createElement("div", { className: "grid-cell u17" },
                            React.createElement("span", { className: "filename" }, file.name)),
                        React.createElement("div", { className: "grid-cell u4" },
                            React.createElement("span", { className: "filesize" }, _this.getFileSize(file.size)))));
            })));
        }
    };
    return Files;
}(React.PureComponent));
exports.default = react_redux_1.connect(function (state) { return state.files; }, // Selects which state properties are merged into the component's props
FileStore.actionCreators // Selects which action creators are merged into the component's props
)(Files); // eslint-disable-line @typescript-eslint/no-explicit-any
//# sourceMappingURL=Files.js.map