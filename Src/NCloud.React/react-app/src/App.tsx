import * as React from 'react';
import { Navigation } from './components/Navigation'
import {
    BrowserRouter as Router,
    Switch,
    Redirect,
    Route
} from "react-router-dom";
import { useRootContext } from './hooks'
import './App.css';
import Files from './pages/Files'

export default function App() {
    const rootConfig = useRootContext()
    return (
        <Router>
            <Navigation />
                <Switch>
                    <Route exact path="/" render={() => {
                        return <Redirect to={`/files/${rootConfig.rootBaseId}/${rootConfig.rootId}`}></Redirect>
                    }} />
                    <Route exact path="/files/:baseId/:id"><Files /></Route>
                </Switch>
        </Router>

    );
}
