import * as React from 'react';
import {useState,useEffect}  from 'react'
import { useRouter } from '../hooks'
import * as FileIcons from '../img/filetypes';
import { IBreadcrumbProps,Card, Elevation, Breadcrumbs, Boundary, Spinner, Intent, Alert } from "@blueprintjs/core";
import { useParams } from 'react-router-dom';
import { getFiles} from '../api'
import { FileInfo, FileType } from '../types'
import {useI18n} from '../hooks'
interface FileParms {
    baseId: string,
    id: string
}
interface FileTableProps {
    loading: boolean,
    children: Array<FileInfo>
}
interface FileSizeProps {
    size: number
}
function FilesTable(props: FileTableProps) {
    const router = useRouter();
    function fileItemClicked(item: FileInfo) {
        if (item.type === FileType.Directory) {
            router.push(`/files/${item.baseId}/${item.id}`);
        } else {

        }
    }
    if (props.loading) {
        return <Spinner intent={Intent.NONE} size={Spinner.SIZE_STANDARD} />
    } else {
        return (
            <React.Fragment>
                {props.children.map((file: FileInfo) =>
                    <Card interactive={true} elevation={Elevation.TWO} key={file.id}>
                        <div className="grid" onClick={() => fileItemClicked(file)}>
                            <div className="grid-cell u3">
                                {renderFileImage(file.ext, file.type === FileType.Directory,file.name)}
                            </div>
                            <div className="grid-cell u17">
                                <span className="filename">{file.name}</span>
                            </div>
                            <div className="grid-cell u4">
                                <FileSize size={file.size} />
                            </div>
                        </div>
                    </Card>
                )}
            </React.Fragment>
        );
    }
}
function FileSize({ size }: FileSizeProps) {
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
        return <span className="filesize">{Math.round(size * 100) / 100 + " " + sizeName[num]}</span>;
    } else {
        return <span className="filesize"></span>;
    }
}
function renderFileImage(type: string, isDirectory: boolean,name:string) {
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
        } else if (type === 'mp3') {
            icon = FileIcons.mp3
        } else if (type === 'json') {
            icon = FileIcons.json
        } else if (type === 'txt') {
            icon = FileIcons.txt
        } else if (type === 'zip' || type === 'gz' || type === 'rar' || type === 'tar') {
            icon = FileIcons.zip
        }
    }
    return <img src={icon} className="fileicon" alt={name}/>
}

export default function Files() {
    const parm = useParams<FileParms>();
    const [error, setError] = useState({ error: false, errorMessage: '' })
    const [breadItems, setBreadItems] = useState<Array<IBreadcrumbProps>>([])
    const [data, setData] = useState<FileTableProps>({
        loading: false,
        children: []
    })
    const {message} = useI18n();
    async function fetchFiles() {
        try{
            let res = await getFiles(parm.baseId, parm.id);
            setData({
                loading: false,
                children: res.children
            })
            setError({
                error: false,
                errorMessage: ''
            })
            setBreadItems([{ icon: "folder-open", text: res.cwd.name, current: true }])
        }
        catch(err){
            console.log(err)
                console.log(typeof err)
                setError({
                    error: true,
                    errorMessage: err
                })
                setData({
                    loading: false,
                    children: []
                })
        }
    }
    useEffect(() => {
        setData((d) => {
            return {
                loading: false,
                children: d.children
            }
        });
        fetchFiles();      
    }, [parm.baseId, parm.id])
    console.log(message)
    return (
        <React.Fragment>
            <Alert
                confirmButtonText={message["ok"]}
                isOpen={error.error}
                onClose={() => setError({ error: false, errorMessage: '' })}
            >
                <p>
                    {error.errorMessage}
                </p>
            </Alert>
            <Card elevation={0} style={{ width: `100%` }}>
                <Breadcrumbs
                    collapseFrom={Boundary.START}
                    items={breadItems}
                />
            </Card>
            <FilesTable {...data} />
        </React.Fragment>
    );
}