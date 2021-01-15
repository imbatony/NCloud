"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.reducer = exports.actionCreators = void 0;
// -----------------
// STATE - This defines the type of data maintained in the Redux store.
var FileType;
(function (FileType) {
    FileType[FileType["Directory"] = 0] = "Directory";
    FileType[FileType["Other"] = 1] = "Other";
})(FileType || (FileType = {}));
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    load: function (baseId, id) { return function (dispatch, getState) {
        // Only load data if it's something we don't already have (and are not already loading)
        var appState = getState();
        if (appState && appState.files && baseId && id) {
            if (baseId != null && baseId != appState.files.baseId && id != null && id != appState.files.id) {
                fetch("/api/file/files/" + baseId + "/" + id)
                    .then(function (response) { return response.json(); })
                    .then(function (res) {
                    var data = res.data;
                    dispatch({ type: 'RECEIVE_FILES_INFO', baseId: baseId, id: id, children: data.children, cwd: data.cwd });
                });
                dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
            }
        }
        else if (appState && appState.files && (!baseId || !id)) {
            fetch("/api/file/root")
                .then(function (response) { return response.json(); })
                .then(function (res) {
                var data = res.data;
                dispatch({ type: 'RECEIVE_FILES_INFO', baseId: data.cwd.baseId, id: data.cwd.id, children: data.children, cwd: data.cwd });
            });
            dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
        }
    }; },
    backword: function () { return function (dispatch, getState) {
        // Only load data if it's something we don't already have (and are not already loading)
        var appState = getState();
        if (appState && appState.files && appState.files.parentBaseId && appState.files.parentId) {
            var parentId_1 = appState.files.parentId;
            var parentBaseId_1 = appState.files.parentBaseId;
            fetch("/api/files/" + parentBaseId_1 + "/" + parentId_1)
                .then(function (response) { return response.json(); })
                .then(function (res) {
                var data = res.data;
                dispatch({ type: 'RECEIVE_FILES_INFO', baseId: parentBaseId_1, id: parentId_1, children: data.children, cwd: data.cwd });
            });
            dispatch({ type: 'REQUEST_FILES_INFO', baseId: parentBaseId_1, id: parentId_1 });
        }
    }; }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var defaultFileState = {
    baseId: undefined,
    id: undefined,
    parentId: undefined,
    parentBaseId: undefined,
    children: [],
    cwd: undefined,
    loading: false
};
var reducer = function (state, incomingAction) {
    if (state === undefined) {
        return defaultFileState;
    }
    var action = incomingAction;
    switch (action.type) {
        case 'REQUEST_FILES_INFO':
            return __assign(__assign({}, state), { loading: true, baseId: action.baseId, id: action.id });
        case 'RECEIVE_FILES_INFO':
            if ((!state.baseId || state.baseId == action.baseId) && (!state.id || state.id == action.id)) {
                return {
                    baseId: action.baseId,
                    id: action.id,
                    parentId: action.cwd.parentId,
                    parentBaseId: action.cwd.parentBaseId,
                    children: action.children,
                    cwd: action.cwd,
                    loading: false
                };
            }
            break;
    }
    return state;
};
exports.reducer = reducer;
//# sourceMappingURL=Files.js.map