import React, { Component } from 'react';
import axios from 'axios';

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
    };
    this.searchServices = this.searchServices.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleBookingSubmit = this.handleBookingSubmit.bind(this);
  }

  componentDidMount() {
    this.fetchServices();
  }

  async fetchServices(pageNumber = 1, pageSize = 3) {
    try {
      const response = await axios.get(`https://localhost:44447/api/CoachingServices?PageNumber=${pageNumber}&PageSize=${pageSize}`);
      this.setState({ services: response.data.items });
    } catch (error) {
      console.error('Error fetching services:', error);
    }
  }

  searchServices(event) {
    const query = event.target.value;
    this.setState({ searchQuery: query });
  }

  handleInputChange(event) {
    const { name, value } = event.target;
    this.setState({
      [name]: value,
    });
  }

  handleBookingSubmit(event) {
    event.preventDefault();
    const { clientName, clientEmail, clientPhone, bookingDate, selectedServiceId } = this.state;
    // Add booking logic here, for example, sending data to the backend
    console.log('Booking details:', { clientName, clientEmail, clientPhone, bookingDate, selectedServiceId });
  }

  render() {
    const { services, searchQuery, clientName, clientEmail, clientPhone, bookingDate } = this.state;
    const filteredServices = services.filter(service =>
      service.name && service.name.toLowerCase().includes(searchQuery.toLowerCase())
    );

    return (
      <div className="container mt-5">
        <h1 className="text-center">Coach Service Booking</h1>
        <div className="input-group mb-3">
          <input
            type="text"
            id="searchService"
            className="form-control"
            placeholder="Search Services"
            value={searchQuery}
            onChange={this.searchServices}
          />
        </div>
        <ul className="list-group" id="serviceList">
          {filteredServices.map(service => (
            <li key={service.id} className="list-group-item">
              <h5>{service.name}</h5>
              <p>{service.description}</p>
              <button
                className="btn btn-primary"
                data-toggle="modal"
                data-target="#bookingModal"
                onClick={() => this.setState({ selectedServiceId: service.id })}
              >
                Book Now
              </button>
            </li>
          ))}
        </ul>

        {/* Booking Modal */}
        <div className="modal fade" id="bookingModal" tabIndex="-1" role="dialog" aria-labelledby="bookingModalLabel" aria-hidden="true">
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title" id="bookingModalLabel">Book Service</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <form id="bookingForm" onSubmit={this.handleBookingSubmit}>
                  <div className="form-group">
                    <label htmlFor="clientName">Name</label>
                    <input
                      type="text"
                      className="form-control"
                      id="clientName"
                      name="clientName"
                      value={clientName}
                      onChange={this.handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="clientEmail">Email</label>
                    <input
                      type="email"
                      className="form-control"
                      id="clientEmail"
                      name="clientEmail"
                      value={clientEmail}
                      onChange={this.handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="clientPhone">Phone</label>
                    <input
                      type="text"
                      className="form-control"
                      id="clientPhone"
                      name="clientPhone"
                      value={clientPhone}
                      onChange={this.handleInputChange}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="bookingDate">Booking Date</label>
                    <input
                      type="date"
                      className="form-control"
                      id="bookingDate"
                      name="bookingDate"
                      value={bookingDate}
                      onChange={this.handleInputChange}
                      required
                    />
                  </div>
                  <input type="hidden" id="selectedServiceId" value={this.state.selectedServiceId} />
                  <button type="submit" className="btn btn-primary">Book Now</button>
                </form>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn btn-secondary" data-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
