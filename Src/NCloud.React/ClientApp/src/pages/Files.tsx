import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link} from 'react-router-dom';
import { ApplicationState } from '../store';
import * as FileStore from '../store/Files';
import folder from '../img/filetypes/folder.png';
import { Card, Elevation } from "@blueprintjs/core";

// At runtime, Redux will merge together...
type FilesProps =
    FileStore.FilesState // ... state we've requested from the Redux store
    & typeof FileStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ baseId: string,id: string }>; // ... plus incoming routing parameters

class Files extends React.PureComponent<FilesProps> {
    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    public render() {
        console.log(folder);
        return (
            <React.Fragment>
                    {this.renderFilesTable()}
            </React.Fragment>
        );
    }
    private goBack() {
        this.props.history.back();
    }
    private redirect(baseId:string,id:string) {
        this.props.history.push(`/files/${baseId}/${id}`);
    }
    private ensureDataFetched() {
        console.log('loading data')
        const baseId = this.props.match.params.baseId || this.props.baseId;
        const id = this.props.match.params.id || this.props.id;
        this.props.load(baseId,id);
    }
    private getFileSize(size: number): string {
        if (size > 0) {
            const sizeName = ["b", "KB", "MB", "GB", "TB"];
            let num = 0;
            while (size > 1024 && num < 5) {
                size = size / 1024;
                num++;
            }
            return size.toFixed(2) + sizeName[num];
        } else {
            return "-";
        }
    }
    private renderFilesTable() {
        console.log('loading data');
        return (
            <React.Fragment>
                {this.props.children.map((file: FileStore.FileInfo) =>
                    <Card interactive={true} elevation={Elevation.TWO} key={file.id}>
                        <div className="container showgrid">  
                            <div className="span-4 border">  
                                <img src={folder} />
                            </div>
                            <div className="span-6">
                                <span>{file.name}</span>    
                            </div>  
                        </div>                      
                    </Card>
                    )}
            </React.Fragment>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.files, // Selects which state properties are merged into the component's props
    FileStore.actionCreators // Selects which action creators are merged into the component's props
)(Files as any); // eslint-disable-line @typescript-eslint/no-explicit-any
