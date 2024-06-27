import React, { Component } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
export class MuscleGroup extends Component {
  static displayName = MuscleGroup.name;

  constructor(props) {
    super(props);
    this.state = {
      muscleGroups: [],
      searchQuery: '',
      createMuscleGroupName: '',
      updateMuscleGroupName: '',
      updateMuscleGroupID: null,
      currentPage: 1,
      itemsPerPage: 5, // Adjust items per page as needed
      detailMuscleGroup: null,
    };
  }

  componentDidMount() {
    // Fetch initial data here
    this.fetchMuscleGroups();
  }

  fetchMuscleGroups = () => {
    // Replace this with your actual data fetching logic
    // For example, using fetch or axios to get data from an API
    const dummyData = [
      { id: 1, name: 'Biceps' },
      { id: 2, name: 'Triceps' },
      { id: 3, name: 'Quadriceps' },
      { id: 4, name: 'Hamstrings' },
      { id: 5, name: 'Deltoids' },
      { id: 6, name: 'Calves' },
      { id: 7, name: 'Pectorals' },
      { id: 8, name: 'Abs' },
      { id: 9, name: 'Obliques' },
      { id: 10, name: 'Glutes' },
      // Add more dummy data as needed
    ];
    this.setState({ muscleGroups: dummyData });
  };

  handleSearchChange = (event) => {
    this.setState({ searchQuery: event.target.value });
  };

  handleCreateChange = (event) => {
    this.setState({ createMuscleGroupName: event.target.value });
  };

  handleUpdateChange = (event) => {
    this.setState({ updateMuscleGroupName: event.target.value });
  };

  handleCreateSubmit = (event) => {
    event.preventDefault();
    const newMuscleGroup = {
      id: this.state.muscleGroups.length + 1,
      name: this.state.createMuscleGroupName,
    };
    this.setState((prevState) => ({
      muscleGroups: [...prevState.muscleGroups, newMuscleGroup],
      createMuscleGroupName: '',
    }));
  };

  handleUpdateSubmit = (event) => {
    event.preventDefault();
    const { updateMuscleGroupID, updateMuscleGroupName, muscleGroups } = this.state;
    const updatedMuscleGroups = muscleGroups.map((group) =>
      group.id === updateMuscleGroupID ? { ...group, name: updateMuscleGroupName } : group
    );
    this.setState({
      muscleGroups: updatedMuscleGroups,
      updateMuscleGroupName: '',
      updateMuscleGroupID: null,
    });
  };

  handleDelete = (id) => {
    const updatedMuscleGroups = this.state.muscleGroups.filter((group) => group.id !== id);
    this.setState({ muscleGroups: updatedMuscleGroups });
  };

  openUpdateModal = (group) => {
    this.setState({
      updateMuscleGroupID: group.id,
      updateMuscleGroupName: group.name,
    });
  };

  openDetailModal = (group) => {
    this.setState({ detailMuscleGroup: group });
  };

  closeDetailModal = () => {
    this.setState({ detailMuscleGroup: null });
  };

  goToPage = (page) => {
    this.setState({ currentPage: page });
  };

  render() {
    const { muscleGroups, searchQuery, currentPage, itemsPerPage, detailMuscleGroup } = this.state;
    const filteredMuscleGroups = muscleGroups.filter((group) =>
      group.name.toLowerCase().includes(searchQuery.toLowerCase())
    );

    // Pagination logic
    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentMuscleGroups = filteredMuscleGroups.slice(indexOfFirstItem, indexOfLastItem);
    const totalPages = Math.ceil(filteredMuscleGroups.length / itemsPerPage);

    return (
      <div className="container mt-5">
        <div className="d-flex justify-content-between mb-4">
          <h2>Muscle Groups</h2>
          <div className="form-inline">
            <input
              type="text"
              id="searchInput"
              className="form-control mr-2"
              placeholder="Search Muscle Group..."
              aria-label="Search Muscle Group"
              value={searchQuery}
              onChange={this.handleSearchChange}
            />
            <button
              className="btn btn-primary mt-2"
              id="createButton"
              data-bs-toggle="modal"
              data-bs-target="#createModal"
            >
              Create Muscle Group
            </button>
          </div>
        </div>
        <table className="table table-bordered">
          <thead>
            <tr>
              <th scope="col">Muscle Group ID</th>
              <th scope="col">Muscle Group Name</th>
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody id="muscleGroupTableBody">
            {currentMuscleGroups.map((group) => (
              <tr key={group.id}>
                <td>{group.id}</td>
                <td>{group.name}</td>
                <td className="d-flex justify-content-around">
                  <button
                    className="btn btn-sm btn-warning mr-2"
                    data-bs-toggle="modal"
                    data-bs-target="#updateModal"
                    onClick={() => this.openUpdateModal(group)}
                  >
                    Update
                  </button>
                  <button
                    className="btn btn-sm btn-danger mr-2"
                    onClick={() => this.handleDelete(group.id)}
                  >
                    Delete
                  </button>
                  <button
                    className="btn btn-sm btn-info"
                    data-bs-toggle="modal"
                    data-bs-target="#detailModal"
                    onClick={() => this.openDetailModal(group)}
                  >
                    Details
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <nav>
          <ul className="pagination justify-content-center">
            <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
              <button
                className="page-link"
                onClick={() => this.goToPage(currentPage - 1)}
                disabled={currentPage === 1}
              >
                Previous
              </button>
            </li>
            {[...Array(totalPages)].map((_, index) => (
              <li
                key={index}
                className={`page-item ${index + 1 === currentPage ? 'active' : ''}`}
                onClick={() => this.goToPage(index + 1)}
              >
                <button className="page-link">{index + 1}</button>
              </li>
            ))}
            <li className={`page-item ${currentPage === totalPages ? 'disabled' : ''}`}>
              <button
                className="page-link"
                onClick={() => this.goToPage(currentPage + 1)}
                disabled={currentPage === totalPages}
              >
                Next
              </button>
            </li>
          </ul>
        </nav>

        {/* Create Muscle Group Modal */}
        <div
          className="modal fade"
          id="createModal"
          tabIndex="-1"
          role="dialog"
          aria-labelledby="createModalLabel"
          aria-hidden="true"
        >
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title" id="createModalLabel">
                  Create Muscle Group
                </h5>
                <button
                  type="button"
                  className="close"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                >
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <form id="createForm" onSubmit={this.handleCreateSubmit}>
                  <div className="form-group mb-3">
                    <label htmlFor="createMuscleGroupName">Muscle Group Name</label>
                    <input
                      type="text"
                      className="form-control"
                      id="createMuscleGroupName"
                      required
                      aria-describedby="createHelp"
                      value={this.state.createMuscleGroupName}
                      onChange={this.handleCreateChange}
                    />
                    <small id="createHelp" className="form-text text-muted">
                      Enter the name of the muscle group.
                    </small>
                  </div>
                  <button type="submit" className="btn btn-primary">
                    Create
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>

        {/* Update Muscle Group Modal */}
        <div
          className="modal fade"
          id="updateModal"
          tabIndex="-1"
          role="dialog"
          aria-labelledby="updateModalLabel"
          aria-hidden="true"
        >
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title" id="updateModalLabel">
                  Update Muscle Group
                </h5>
                <button
                  type="button"
                  className="close"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                >
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <form id="updateForm" onSubmit={this.handleUpdateSubmit}>
                  <input type="hidden" id="updateMuscleGroupID" value={this.state.updateMuscleGroupID} />
                  <div className="form-group mb-3">
                    <label htmlFor="updateMuscleGroupName">Muscle Group Name</label>
                    <input
                      type="text"
                      className="form-control"
                      id="updateMuscleGroupName"
                      required
                      aria-describedby="updateHelp"
                      value={this.state.updateMuscleGroupName}
                      onChange={this.handleUpdateChange}
                    />
                    <small id="updateHelp" className="form-text text-muted">
                      Enter the new name for the muscle group.
                    </small>
                  </div>
                  <button type="submit" className="btn btn-primary">
                    Update
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>

        {/* Detail Muscle Group Modal */}
        <div
          className="modal fade"
          id="detailModal"
          tabIndex="-1"
          role="dialog"
          aria-labelledby="detailModalLabel"
          aria-hidden="true"
        >
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title" id="detailModalLabel">
                  Muscle Group Details
                </h5>
                <button
                  type="button"
                  className="close"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                  onClick={this.closeDetailModal}
                >
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body">
                {detailMuscleGroup && (
                  <div>
                    <p><strong>ID:</strong> {detailMuscleGroup.id}</p>
                    <p><strong>Name:</strong> {detailMuscleGroup.name}</p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
