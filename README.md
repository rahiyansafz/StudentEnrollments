# StudentEnrollmentsAPI

This project is an API for managing student course enrollments. It is built with .NET and follows an n-layer architecture with generic functionality.

## Layers

### API

The API layer handles HTTP requests and responses. It includes the following controllers:

- AuthController: Endpoints for user authentication
- CourseController: Endpoints for managing courses
- EnrollmentController: Endpoints for managing course enrollments
- StudentController: Endpoints for managing students

### Data

The Data layer handles database access and business logic. It includes the following repositories:

- CourseRepository: Access to course data
- EnrollmentRepository: Access to enrollment data
- StudentRepository: Access to student data

## Endpoints

### Authentication

The following endpoints are available for user authentication:

- POST /api/auth/login: Authenticate user and return a JWT token
- POST /api/auth/register: Register a new user

### Courses

The following endpoints are available for managing courses:

- GET /api/courses: Get a list of all courses
- GET /api/courses/{id}: Get details for a specific course
- POST /api/courses: Create a new course
- PUT /api/courses/{id}: Update an existing course
- DELETE /api/courses/{id}: Delete a course

### Enrollments

The following endpoints are available for managing course enrollments:

- GET /api/enrollments: Get a list of all enrollments
- GET /api/enrollments/{id}: Get details for a specific enrollment
- POST /api/enrollments: Create a new enrollment
- PUT /api/enrollments/{id}: Update an existing enrollment
- DELETE /api/enrollments/{id}: Delete an enrollment

### Students

The following endpoints are available for managing students:

- GET /api/students: Get a list of all students
- GET /api/students/{id}: Get details for a specific student
- POST /api/students: Create a new student
- PUT /api/students/{id}: Update an existing student
- DELETE /api/students/{id}: Delete a student

## Authentication

All endpoints except for /api/auth/register require authentication. To authenticate, send a POST request to /api/auth/login with a JSON body containing the following fields:

- email: User email
- password: User password

The response will contain a JWT token that must be included in the Authorization header of subsequent requests. For example:

Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c

## License

This project is licensed under the MIT License - see the LICENSE file for details.
