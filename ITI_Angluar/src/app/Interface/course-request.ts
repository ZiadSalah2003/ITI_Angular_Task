export interface StudentDegreeRequest {
  studentId: string;
  degree: number;
}

export interface CourseRequest {
  crs_Name: string;
  crs_Description: string;
  duration: number;
  departmentId?: number | null;
  studentDegrees?: StudentDegreeRequest[];
}

export interface AssignCourseDegreesRequest {
  departmentId: number;
  courseId: number;
  studentDegrees: StudentDegreeRequest[];
}