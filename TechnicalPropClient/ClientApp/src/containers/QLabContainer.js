import React, {Component} from 'react';
const QLAB = "api/QLab/";
const WORKSPACE = "api/Workspace/";
const handleRequest = (request, onSuccess, onError) => {
    request.then(function(response) {
        if (response.status !== 200) {
            onError();
        }
        else {
            onSuccess();
        }
    })
    .catch(onError);
}
export class QLabContainer {
    
    connect(ipAddress, onSuccess, onError) {
        handleRequest(fetch(QLAB+"Connect?ipAddress=" + ipAddress,
        {
            method: "post"
        }), onSuccess, onError);        
    }
    getWorkspaces(onSuccess, onError) {
        fetch(WORKSPACE+"Get", 
        {
            method: "get",
        }).then(onSuccess)
        .catch(onError);
    }
    connectToWorkspace(workspaceId, onSuccess, onError) {
        fetch(WORKSPACE+"Connect?workspaceId=" + workspaceId, {
            method: "post"
        }).then(onSuccess)
        .catch(onError);
    }
    disconnectWorkspace(workspaceId, onSuccess, onError) {
        fetch(WORKSPACE+"Disconnect", {
            method: "post"
        }).then(onSuccess)
        .catch(onError);
    }
}

export default QLabContainer;