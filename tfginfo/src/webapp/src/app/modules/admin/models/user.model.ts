export interface UserBase {
    id?: number;
    username: string;
    password?: string;
    auth_code?: string;
}

export interface UserDTO extends UserBase {
    role: RoleBase;
    token?: string;
}

export interface UserFlatDTO extends UserBase {
    roleId: number;
}

export interface NewUserDTO {
    user: AppUserDTO;
    firstLogin: boolean;
}

export interface AppUserDTO {
    id: number;
    username: string;
    token: string;
    universitiesId: number[];
    department?: number;
    career?: number;
    role: RoleBase;
}

export interface RoleBase {
    id: number;
    name: string;
}

export interface UserCredentials {
    username: string;
    password: string;
}

export interface UserChangePassword {
    username: string;
    OldPassword: string;
    NewPassword: string;
}