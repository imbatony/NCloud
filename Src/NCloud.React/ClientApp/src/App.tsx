import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Files from './pages/Files';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Files} />
        <Route path='/files/:baseId?/:id?' component={Files} />
    </Layout>
);
