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
exports.Navigation = void 0;
var React = require("react");
var core_1 = require("@blueprintjs/core");
var Navigation = /** @class */ (function (_super) {
    __extends(Navigation, _super);
    function Navigation() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Navigation.prototype.render = function () {
        return (React.createElement(core_1.Navbar, { className: core_1.Classes.DARK },
            React.createElement(core_1.NavbarGroup, { align: core_1.Alignment.CENTER },
                React.createElement(core_1.NavbarHeading, null, "NCloud"))));
    };
    return Navigation;
}(React.PureComponent));
exports.Navigation = Navigation;
//# sourceMappingURL=Navigation.js.map