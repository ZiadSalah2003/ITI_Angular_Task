export interface StudentDegree {
  studentId: string;
  studentName?: string | null;
  degree: number;
}

export interface Course {
  id: number;
  crs_Name?: string | null;
  crs_Description?: string | null;
  duration: number;
  departmentId?: number | null;
  departmentName?: string | null;
  studentDegrees: StudentDegree[];
}