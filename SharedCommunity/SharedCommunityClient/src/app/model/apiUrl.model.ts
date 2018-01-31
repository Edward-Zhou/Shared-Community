export class APIUrl {    
    //login
    accountLogin = () => `/token/auth`
    //Thread
    getThreads = (startTime: string, endTime: string) => `api/MSDNForum/Threads/${startTime}/${endTime}` 
}