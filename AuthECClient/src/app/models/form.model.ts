export interface Gender {
    value: string;
    label: string;
  }

  export interface Role {
    value: string;
    label: string;
  }

  export const GENDERS: Gender[] = [
    { value: 'male', label: 'Male' },
    { value: 'female', label: 'Female' },
    // { value: 'mail', label: 'Mail' }
  ];

  export const ROLES: Role[] = [
    { value: 'Admin', label: 'Admin' },
    { value: 'Teacher', label: 'Teacher' },
     { value: 'Student', label: 'Student' }
  ];