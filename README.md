# RaceDayPart3
# RaceDay – Event Management Web Application

## 1. Project Overview

RaceDay is a web-based event management platform developed to manage running, walking and cycling events. The system provides a central platform where event organisers can manage sporting events and participants can register, enrol in events and view their race results.

Part 3 focuses on developing the front-end application using ASP.NET Core MVC. The MVC application communicates with the RESTful API developed in Part 2. This approach separates the user interface from the business logic and database operations.

The application provides different interfaces for Organisers and Participants. It also integrates Azure Blob Storage for image uploads and uses Docker to containerise the application.

The main goal is to create a secure, user-friendly and responsive web application that brings all the RaceDay components together.

## 2. Project Objectives

The objectives of RaceDay Part 3 are to:

- Develop an ASP.NET Core MVC web application.
- Connect the MVC application to the existing Part 2 RESTful API.
- Implement registration and login functionality.
- Maintain authenticated user sessions across application pages.
- Provide separate dashboards and navigation for Organisers and Participants.
- Allow Organisers to manage events, categories, enrolments and results.
- Allow Participants to browse events, enrol and view their personal results.
- Integrate Azure Blob Storage through the existing API for image uploads.
- Use Docker to containerise the MVC application.
- Implement a consistent, responsive and user-friendly interface.
- Extend GitHub Actions to build and test the MVC application.

## 3. Technologies Used

| Technology | Purpose |
|---|---|
| C# | Main programming language |
| ASP.NET Core MVC | Development of the web application |
| ASP.NET Core Web API | Handles application data and business operations |
| Entity Framework Core | Database management within the API |
| Microsoft SQL Server | Stores application information |
| HTML5 | Structures web pages |
| CSS3 | Styles the application |
| Bootstrap | Responsive layouts and interface components |
| JavaScript | Adds interactive functionality |
| Azure Blob Storage | Stores uploaded images |
| Docker | Containerises the MVC application |
| Visual Studio | Development environment |
| Git and GitHub | Source control |
| GitHub Actions | Continuous integration and automated builds |
| Swagger | API documentation and testing |

## 4. System Architecture

RaceDay follows a layered architecture consisting of three main components.

### 4.1 MVC Web Application

The MVC application provides the interface that users interact with. It displays information, collects user input and sends HTTP requests to the API.

It does not access SQL Server directly and does not duplicate the API's business logic.

### 4.2 RESTful API

The API acts as the communication layer between the MVC application and the database. It handles authentication, validation, event management, enrolments, categories and race results.

### 4.3 Database and Azure Blob Storage

SQL Server stores structured information such as users, events, categories, enrolments and results.

Azure Blob Storage stores uploaded images, such as event banners and participant profile pictures. All image uploads and downloads must be handled through the Part 2 API. The MVC application must not communicate directly with Azure Blob Storage.

## 5. Authentication and Role-Based Navigation

RaceDay supports two types of users: Organisers and Participants.

Users can register and log in through the MVC application. The MVC application communicates with the Part 2 API to authenticate users.

After successful login, the application maintains the authenticated user's state and role using session-based authentication.

### Organiser access

Organisers are provided with navigation options for:

- Dashboard
- Manage Events
- Manage Categories
- View Enrolments
- Enter Results
- Logout

### Participant access

Participants are provided with navigation options for:

- Home
- Browse Events
- My Enrolments
- My Results
- My Profile
- Logout

The system must prevent users from accessing pages that do not belong to their roles.

## 6. Organiser Interface

The Organiser interface provides tools for managing sporting events.

### 6.1 Organiser Dashboard

The dashboard provides an overview of the organiser's activities, including:

- Total events created.
- Total enrolments per event.
- Upcoming event dates.
- A summary of managed events.

### 6.2 Event Management

Organisers can create, edit and delete sporting events.

Each event contains the following information:

- Event name.
- Description.
- Event date.
- Location.
- Distance.
- Event type.
- Event banner image.

### 6.3 Category Management

Organisers can create and manage categories for individual events.

Categories can be based on age groups or race distances, depending on the event requirements.

### 6.4 Enrolment Management

The enrolments page allows Organisers to view participants registered for a particular event.

The page displays participant information, selected categories and enrolment statuses.

### 6.5 Results Management

Organisers can capture and publish race results after an event has concluded.

Results include:

- Participant information.
- Event name.
- Finishing time.
- Finishing position.

This information allows participants to view their performance and race history.

## 7. Participant Interface

The Participant interface allows users to explore events and manage their participation.

### 7.1 Home Page

The home page displays upcoming sporting events. Participants can browse and filter events according to available options.

### 7.2 Event Details

Participants can view complete event information, including:

- Event name and description.
- Date and location.
- Distance and event type.
- Available categories.
- Event banner image.

### 7.3 Event Enrolment

Logged-in Participants can select an event and choose an available category.

Once the enrolment is submitted successfully, the application communicates with the API to save the enrolment immediately.

### 7.4 My Enrolments

This page displays all events the participant has entered, together with their selected categories and enrolment statuses.

Different colours are used to identify statuses. For example:

- Green: Confirmed.
- Amber: Pending.
- Red: Cancelled, where applicable.

### 7.5 My Results

Participants can view their personal race history across completed events.

The results page displays:

- Event name.
- Event date.
- Selected category.
- Recorded finishing time.
- Finishing position.

For example, a participant's position may be displayed as 47th out of 312 finishers.

### 7.6 Profile Management

Participants can view and update their personal information.

They can also upload a profile picture. The image is sent through the API and stored using Azure Blob Storage.

## 8. Azure Blob Storage Integration

Azure Blob Storage is used to store images uploaded through the RaceDay application.

The system supports:

- Event banner image uploads.
- Participant profile picture uploads.
- Retrieving stored images for display.

The integration takes place through the Part 2 API.

The MVC application sends the image to an API endpoint. The API handles the file validation and upload process and communicates with Azure Blob Storage.

This design improves security and maintains a clear separation between the front end and the storage service.

The application should validate image file types and sizes before accepting uploads. Azure connection strings and access keys must be stored securely and must not be committed to GitHub.

## 9. Docker Containerisation

Docker is used to package the MVC application and its dependencies into a container.

Containerisation makes it easier to run the application consistently across different environments.

The project includes a Dockerfile that defines how the application is built and started.

### Building the Docker Image

Run the following command from the directory containing the Dockerfile:

```bash
docker build -t raceday-mvc .
```

### Running the Docker Container

The following is an example command for running the MVC application on port 8080:

```bash
docker run --rm -p 8080:8080 raceday-mvc
```

The API address and other required settings must be supplied through the application's configuration or environment variables.

The API must also be reachable from inside the container. The application should behave consistently whether it is running locally or inside Docker.

## 10. User Interface Design

The RaceDay interface follows a sporting and energetic visual theme.

The design uses consistent colours, typography, spacing and navigation throughout the application.

Bootstrap is used to create responsive layouts that work on desktop computers, tablets and mobile devices.

The interface includes:

- A consistent navigation menu.
- Organiser and Participant dashboards.
- Responsive tables and forms.
- Clear buttons and action links.
- Colour-coded enrolment statuses.
- User-friendly validation messages.
- Consistent fonts and colours.
- Appropriate error messages for invalid input.

The application should handle invalid information gracefully without crashing or displaying technical error messages to users.

## 11. API Integration

The MVC application communicates with the Part 2 RESTful API for all data operations.

The API is responsible for handling the application's business logic and database access.

The MVC application uses HTTP requests to retrieve, create, update and delete information.

Examples of API operations include:

| Operation | Purpose |
|---|---|
| User registration | Create a new account |
| User login | Authenticate a user |
| Event retrieval | Display available events |
| Event creation | Allow Organisers to create events |
| Event updates | Modify existing event information |
| Category management | Add or update event categories |
| Enrolment | Register Participants for events |
| Results | Record and retrieve race results |
| Image uploads | Send images to the API for Azure storage |

This architecture prevents unnecessary duplication of business logic and database operations.

## 12. GitHub and Continuous Integration

GitHub is used to store the RaceDay source code and track development progress.

The project is maintained in a clearly named branch or folder for Part 3.

At least 20 meaningful commits are required to demonstrate the development process.

GitHub Actions is used to automate the build process. The existing workflow from Part 2 must be updated to include the MVC project.

The workflow should:

1. Check out the repository.
2. Install the required .NET SDK.
3. Restore NuGet packages.
4. Build the Part 2 API.
5. Build the Part 3 MVC application.
6. Run available automated tests.

A screenshot showing a successful GitHub Actions build should be included in the README after the workflow has completed successfully.

## 13. Testing

Testing is performed to ensure that the application works correctly and provides a reliable experience.

The following areas should be tested:

- User registration and login.
- Session management.
- Organiser and Participant role restrictions.
- Event creation, editing and deletion.
- Category management.
- Participant enrolments.
- Results entry and display.
- Profile updates.
- Event banner uploads.
- Profile picture uploads.
- Invalid input handling.
- API communication.
- Docker container execution.

The application should also be tested locally and inside Docker to confirm that both environments behave consistently.


