import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Counter from './pages/Counter';
import FetchData from './pages/FetchData';
import Files from './pages/Files';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Files} />
        <Route path='/counter' component={Counter} />
        <Route path='/files/:baseId?/:id?' component={Files} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
    </Layout>
);
