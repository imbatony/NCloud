import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as FileStore from '../store/Files';
import * as FileIcons from './img/filetypes';
import { Card, Elevation, Breadcrumbs, Boundary, Spinner, Intent, Alert } from "@blueprintjs/core";

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
        return (
            <React.Fragment>
                <Alert
                    confirmButtonText="好的"
                    isOpen={this.props.error}
                    onClose={this.props.closeError}
                >
                    <p>
                        {this.props.errorMessage}
                    </p>
                </Alert>
                <Card elevation={0} style={{ width: `100%` }}>
                    <Breadcrumbs
                        collapseFrom={Boundary.START}
                        items={this.props.breadItems}
                    />
                </Card>
                    {this.renderFilesTable()}
            </React.Fragment>
        );
    }
    private goBack() {
        this.props.history.goBack();
    }
    private redirect(baseId:string,id:string) {
        this.props.history.push(`/files/${baseId}/${id}`);
    }

    private onFileItemClicked(item: FileStore.FileInfo) {
        if (item.type == FileStore.FileType.Directory) {
            this.redirect(item.baseId, item.id);
        } else {

        }
    }
    private ensureDataFetched() {
        if (!this.props.loading) {
            const baseId = this.props.match.params.baseId || this.props.baseId;
            const id = this.props.match.params.id || this.props.id;
            console.log("baseId:", baseId);
            console.log("id:", id);
            this.props.load(baseId, id);
        }
    }
    private getFileSize(size: number): string {
        if (size > 0) {
            const sizeName = ["b", "KB", "MB", "GB", "TB"];
            let num = 0;
            while (size > 1024 && num < 5) {
                size = size / 1024;
                num++;
            }
            if (num < 1) {
                num = 1;
                size = 1;
            }
            return Math.round(size * 100) / 100 +" "+ sizeName[num];
        } else {
            return "";
        }
    }
    private renderFileImage(type: string, isDirectory: boolean) {

        let icon = FileIcons.file;
        if (isDirectory) {
            icon = FileIcons.folder;
        } else {
            if (type === 'mp4') {
                icon = FileIcons.mp4;
            } else if (type === 'avi') {
                icon = FileIcons.avi;
            } else if (type === 'exe') {
                icon = FileIcons.exe;
            } else if (type === 'png' || type === 'jpg' || type === 'svg') {
                icon = FileIcons.jpg;
            } else if (type === 'html' || type === 'htm') {
                icon = FileIcons.html
            } else if (type === 'pdf') {
                icon = FileIcons.pdf
            } else if (type === 'doc' || type === 'docx') {
                icon = FileIcons.doc
            } else if (type === 'ppt' || type === 'pptx') {
                icon = FileIcons.ppt
            } else if (type === 'xls' || type === 'xlsx') {
                icon = FileIcons.xls
            }else if (type === 'mp3') {
                icon = FileIcons.mp3
            } else if (type === 'json') {
                icon = FileIcons.json
            } else if (type === 'txt') {
                icon = FileIcons.txt
            } else if (type === 'zip' || type === 'gz' || type === 'rar' || type === 'tar') {
                icon = FileIcons.zip
            }
        }
        return <img src={icon} className="fileicon" />
       
    }
    private renderFilesTable() {
        if (this.props.loading) {
            console.log('loading data');
            return <Spinner intent={Intent.NONE} size={Spinner.SIZE_STANDARD}/>
        } else {
            return (
                <React.Fragment>
                    {this.props.children.map((file: FileStore.FileInfo) =>
                        <Card interactive={true} elevation={Elevation.TWO} key={file.id}>
                            <div className="grid" onClick={() => this.onFileItemClicked(file)}>
                                <div className="grid-cell u3">
                                    {this.renderFileImage(file.ext, file.type == FileStore.FileType.Directory)}
                                </div>
                                <div className="grid-cell u17">
                                    <span className="filename">{file.name}</span>
                                </div>
                                <div className="grid-cell u4">
                                    <span className="filesize">{this.getFileSize(file.size)}</span>
                                </div>
                            </div>
                        </Card>
                    )}
                </React.Fragment>
            );
        }
    }
}

export default connect(
    (state: ApplicationState) => state.files, // Selects which state properties are merged into the component's props
    FileStore.actionCreators // Selects which action creators are merged into the component's props
)(Files as any); // eslint-disable-line @typescript-eslint/no-explicit-any
