export interface Filter {
    key: string;
    value: string;
}

export interface ImportResponse {
    success: number;
    errorItems: string[];
}