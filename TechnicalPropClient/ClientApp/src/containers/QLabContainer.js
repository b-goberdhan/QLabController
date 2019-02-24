import React from 'react';

export class QLabContainer extends React.Component {
    
    static connect(ipAddress, onSuccess, onError) {
        fetch("api/QLab/Connect?ipAddress=" + ipAddress,
        {
            method: "post"
        }).then(function(response){
            if (response.status !== 200) {
                onError();
            }
            else {
                onSuccess();
            }
        })
        .catch(onError);
    }
    static getWorkspaces(onSuccess, onError) {
        fetch("api/QLab/GetWorkspaces", 
        {
            method: "get",
        }).then(onSuccess)
        .catch(onError);
    }
    static connectToWorkspace(workspaceId, onSuccess, onError) {
        fetch("api/QLab/ConnectToWorkspace?workspacesId=" + workspaceId, {
            method: "post"
        }).then(onSuccess)
        .catch(onError);
    }
}

export default QLabContainer;