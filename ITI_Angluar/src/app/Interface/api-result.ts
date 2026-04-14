export interface ApiError {
  code: string;
  description: string;
  statusCode: number | null;
}

export interface ApiResult<T> {
  value: T;
  isSuccess: boolean;
  isFailure: boolean;
  error: ApiError;
}
