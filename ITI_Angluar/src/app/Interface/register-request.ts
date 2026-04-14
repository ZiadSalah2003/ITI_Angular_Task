export interface RegisterRequest {
    name:string;
    email:string;
    password:string;
    age:number;
    departmentId:number | null;
}
