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
var folder_png_1 = require("../img/filetypes/folder.png");
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
        console.log(folder_png_1.default);
        return (React.createElement(React.Fragment, null, this.renderFilesTable()));
    };
    Files.prototype.goBack = function () {
        this.props.history.goBack();
    };
    Files.prototype.redirect = function (baseId, id) {
        this.props.history.push("/files/" + baseId + "/" + id);
    };
    Files.prototype.ensureDataFetched = function () {
        var baseId = this.props.match.params.baseId || this.props.baseId;
        var id = this.props.match.params.id || this.props.id;
        this.props.load(baseId, id);
    };
    Files.prototype.getFileSize = function (size) {
        if (size > 0) {
            var sizeName = ["b", "KB", "MB", "GB", "TB"];
            var num = 0;
            while (size > 1024 && num < 5) {
                size = size / 1024;
                num++;
            }
            return size.toFixed(2) + sizeName[num];
        }
        else {
            return "-";
        }
    };
    Files.prototype.renderFilesTable = function () {
        var _this = this;
        return (React.createElement(React.Fragment, null, this.props.children.map(function (file) {
            return React.createElement(core_1.Card, { interactive: true, elevation: core_1.Elevation.TWO },
                React.createElement("p", null,
                    React.createElement("img", { src: folder_png_1.default }),
                    file.name),
                React.createElement("span", null, _this.getFileSize(file.size)));
        })));
    };
    return Files;
}(React.PureComponent));
exports.default = react_redux_1.connect(function (state) { return state.files; }, // Selects which state properties are merged into the component's props
FileStore.actionCreators // Selects which action creators are merged into the component's props
)(Files); // eslint-disable-line @typescript-eslint/no-explicit-any
//# sourceMappingURL=Files.js.map