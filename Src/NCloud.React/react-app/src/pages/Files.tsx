import * as React from 'react';
import { useState, useEffect } from 'react'
import { useRouter } from '../hooks'
import * as FileIcons from '../img/filetypes';
import { Popover, IBreadcrumbProps, Card, Elevation, Breadcrumbs, Boundary, Spinner, Intent, Alert, Button, Menu, MenuItem, MenuDivider } from "@blueprintjs/core";
import { useParams } from 'react-router-dom';
import { getFileDownloadUrl, getFiles, getFileViewUrl } from '../api'
import { FileInfo, FileType } from '../types'
import { useI18n } from '../hooks'
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

function FileOperations({ file }: { file: FileInfo }) {
    const { message } = useI18n();
    if (file.type == FileType.Directory) {
        return (
            null
        )
    } else {
        const menu = (
            <Menu>               
                <MenuItem icon="info-sign" text={`${message['filesize']}: ${getFileSize(file)}`}  />
                <MenuItem icon="download" text={message["download"]} onClick={(e: React.MouseEvent<HTMLElement>) => { window.open(getFileDownloadUrl(file), '_blank'); }} />
            </Menu>
        )
        return (
            <Popover content={menu} placement="right-end">
                <Button icon="more" />
            </Popover>
        )
    }
}

function FilesTable(props: FileTableProps) {
    const router = useRouter();
    function fileItemClicked(e: React.MouseEvent<HTMLElement>, item: FileInfo) {
        if (item.type === FileType.Directory) {
            router.push(`/files/${item.baseId}/${item.id}`);
        } else {
            window.open(getFileViewUrl(item), '_blank')
        }
    }
    if (props.loading) {
        return <Spinner intent={Intent.NONE} size={Spinner.SIZE_STANDARD} />
    } else {
        return (
            <React.Fragment>
                {props.children.map((file: FileInfo) =>
                    <Card interactive={true} elevation={Elevation.ONE} key={file.id}>
                        <div className="grid fileline" onClick={(e: React.MouseEvent<HTMLElement>) => fileItemClicked(e, file)}>
                            <div className="grid-cell u3">
                                {renderFileImage(file.ext, file.type === FileType.Directory, file.name)}
                            </div>
                            <div className="grid-cell u17" style={{justifyContent:"left"}}>
                                <span className="filename">{file.name}</span>
                            </div>
                            <div className="grid-cell u4" onClick={(e) => { e.stopPropagation(); }}>
                                <FileOperations file={file} />
                            </div>
                        </div>
                    </Card>

                )}
            </React.Fragment>
        );
    }
}
function getFileSize({ size }: FileSizeProps):string {
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
        return Math.round(size * 10) / 10 + " " + sizeName[num];
    } else {
        return '';
    }
}
function renderFileImage(type: string, isDirectory: boolean, name: string) {
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
    return <img src={icon} className="fileicon" alt={name} />
}

export default function Files() {
    const parm = useParams<FileParms>();
    const [error, setError] = useState({ error: false, errorMessage: '' })
    const [breadItems, setBreadItems] = useState<Array<IBreadcrumbProps>>([])
    const [data, setData] = useState<FileTableProps>({
        loading: false,
        children: []
    })
    const { message } = useI18n();
    async function fetchFiles() {
        try {
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
        catch (err) {
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