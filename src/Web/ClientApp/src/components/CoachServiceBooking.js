import React, { Component } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

export class CoachServiceBooking extends Component {
  constructor(props) {
    super(props);
    this.state = {
      services: [],
      searchQuery: '',
      clientName: '',
      clientEmail: '',
      clientPhone: '',
      bookingDate: '',
      selectedServiceId: null,
      successMessage: '',
      selectedServiceDetails: null,
      showModal: false,
    };
    this.searchServices = this.searchServices.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleDetailClick = this.handleDetailClick.bind(this);
    this.handleCloseModal = this.handleCloseModal.bind(this);
  }

  componentDidMount() {
    this.fetchServices();
  }

  async fetchServices(pageNumber = 1, pageSize = 5) {
    try {
      const response = await axios.get(`https://localhost:44447/api/CoachingServices?PageNumber=${pageNumber}&PageSize=${pageSize}`);
      this.setState({ services: response.data.items });
    } catch (error) {
      console.error('Error fetching services:', error);
    }
  }

  searchServices(event) {
    const query = event.target.value.toLowerCase();
    this.setState({ searchQuery: query });
  }

  handleInputChange(event) {
    const { name, value } = event.target;
    this.setState({
      [name]: value,
    });
  }

  async handleDetailClick(serviceId) {
    try {
      const response = await axios.get(`https://localhost:44447/api/CoachingServices/${serviceId}`);
      console.log(response);
      this.setState({
        selectedServiceId: serviceId,
        selectedServiceDetails: response.data, // Accessing the data property from the response
        successMessage: 'Detail fetched successfully!',
        showModal: true,
      });
    } catch (error) {
      console.error('Detail fetch error:', error.response?.data || error.message);
    }
  }

  handleCloseModal() {
    this.setState({ showModal: false });
  }

  render() {
    const { services, searchQuery, clientName, clientEmail, clientPhone, bookingDate, successMessage, selectedServiceDetails, showModal } = this.state;
    const filteredServices = services.filter(service => {
      const { id, serviceName, description, duration, price } = service;
      const query = searchQuery.toLowerCase();
      return (
        id.toString().includes(query) ||
        serviceName.toLowerCase().includes(query) ||
        description.toLowerCase().includes(query) ||
        duration.toString().includes(query) ||
        price.toString().includes(query)
      );
    });

    return (
      <div className="container mt-5">
        <h1 className="text-center" style={{ fontSize: '40px' }}><strong>Coach Service Detail</strong> </h1>
        <div className="input-group mb-3">
          <input
            type="text"
            id="searchService"
            className="form-control"
            style={{ height: '50px', marginTop: '30px' }}
            placeholder="Search Services"
            value={searchQuery}
            onChange={this.searchServices}
          />
        </div>
        <ul className="list-group" id="serviceList">
          {filteredServices.map(service => (
            <li key={service.id} className="list-group-item d-flex justify-content-between align-items-center" style={{ height: '110px' }}>
              <div>
                <strong> Service: {service.id}</strong> - {service.serviceName} - {service.description} (Duration: {service.duration} mins, Price: ${service.price})
              </div>
              <button
                className="btn btn-primary"
                style={{ width: '140px' }}
                onClick={() => this.handleDetailClick(service.id)}
              >
                Detail
              </button>
            </li>
          ))}
        </ul>
        {/* Detail Modal */}
        {showModal && (
          <div className="modal fade show" tabIndex="-1" role="dialog" style={{ display: 'block' }} aria-labelledby="detailModalLabel" aria-hidden="true">
            <div className="modal-dialog" role="document">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title" id="detailModalLabel">Service Detail</h5>
                  <button type="button" className="close" aria-label="Close" onClick={this.handleCloseModal}>
                    <span aria-hidden="true">&times;</span>
                  </button>
                </div>
                <div className="modal-body">
                  {selectedServiceDetails ? (
                    <div>
                      <h3><strong>Service Name:</strong> {selectedServiceDetails.serviceName}</h3>
                      <h4><strong>Description</strong>: {selectedServiceDetails.description}</h4>
                      <p><strong>Duration:</strong> {selectedServiceDetails.duration} mins</p>
                      <p><strong>Price:</strong> ${selectedServiceDetails.price}</p>
                      <p><strong>Availability:</strong> {selectedServiceDetails.serviceAvailability ? 'Available' : 'Not Available'}</p>
                      <p><strong>Announcement:</strong> {selectedServiceDetails.availabilityAnnouncement}</p>
                      <p><strong>Created By:</strong> {selectedServiceDetails.createdByUserName}</p>
                      <p><strong>Last Modified By:</strong> {selectedServiceDetails.lastModifiedByUserName}</p>
                      {/*<form id="detailForm">*/}
                      {/*  <div className="form-group">*/}
                      {/*    <label htmlFor="clientName">Name</label>*/}
                      {/*    <input*/}
                      {/*      type="text"*/}
                      {/*      className="form-control"*/}
                      {/*      id="clientName"*/}
                      {/*      name="clientName"*/}
                      {/*      value={clientName}*/}
                      {/*      onChange={this.handleInputChange}*/}
                      {/*      required*/}
                      {/*    />*/}
                      {/*  </div>*/}
                      {/*  <div className="form-group">*/}
                      {/*    <label htmlFor="clientEmail">Email</label>*/}
                      {/*    <input*/}
                      {/*      type="email"*/}
                      {/*      className="form-control"*/}
                      {/*      id="clientEmail"*/}
                      {/*      name="clientEmail"*/}
                      {/*      value={clientEmail}*/}
                      {/*      onChange={this.handleInputChange}*/}
                      {/*      required*/}
                      {/*    />*/}
                      {/*  </div>*/}
                      {/*  <div className="form-group">*/}
                      {/*    <label htmlFor="clientPhone">Phone</label>*/}
                      {/*    <input*/}
                      {/*      type="text"*/}
                      {/*      className="form-control"*/}
                      {/*      id="clientPhone"*/}
                      {/*      name="clientPhone"*/}
                      {/*      value={clientPhone}*/}
                      {/*      onChange={this.handleInputChange}*/}
                      {/*      required*/}
                      {/*    />*/}
                      {/*  </div>*/}
                      {/*  <div className="form-group">*/}
                      {/*    <label htmlFor="bookingDate">Booking Date</label>*/}
                      {/*    <input*/}
                      {/*      type="date"*/}
                      {/*      className="form-control"*/}
                      {/*      id="bookingDate"*/}
                      {/*      name="bookingDate"*/}
                      {/*      value={bookingDate}*/}
                      {/*      onChange={this.handleInputChange}*/}
                      {/*      required*/}
                      {/*    />*/}
                      {/*  </div>*/}
                      {/*  <input type="hidden" id="selectedServiceId" value={this.state.selectedServiceId || ''} />*/}
                      {/*</form>*/}
                    </div>
                  ) : (
                    <div>Loading...</div>
                  )}
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-secondary" onClick={this.handleCloseModal}>Close</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    );
  }
}

export default CoachServiceBooking;
