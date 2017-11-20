export class UserInfo{
    userName: string
}

export class UserLoginResponse{
    userInfo: UserInfo;
    access_token: string
}