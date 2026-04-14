export interface PagedValue<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  hasPreviosPage: boolean;
  hasNextPage: boolean;
}
