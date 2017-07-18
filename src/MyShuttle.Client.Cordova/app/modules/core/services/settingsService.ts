﻿/// <reference path="../../../_app.ts" />
/// <reference path="../core.ts" />

module MyShuttle.Core {
    export class Vehicle {
        VehicleId: number;
        CarrierId: number;
        DriverId: number;
        Rate: number;
    }

    export class Coordinate {
        latitude: number;
        longitude: number;

        constructor(latitude: number, longitude: number) {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }

    export class SettingsService {
        public defaultEmployeeEmail: string;
        public vehicle: Vehicle;
        public startRideLocation: Coordinate;
        public endRideLocation: Coordinate;
        public rideDistance: number;
        public rideAddress: string;
        public bingMapsKey: string;
        public mobileServiceKey: string;
        public gcmSenderId: string;
        public realTimeNotificationsServerUrl: string;

        constructor(private storageService: MyShuttle.Core.StorageService) {
            this.defaultEmployeeEmail = '';

            this.vehicle = new Vehicle();
            this.vehicle.VehicleId = 5;
            this.vehicle.DriverId = 5;
            this.vehicle.CarrierId = 1;
            this.vehicle.Rate = 3;

            var coord = new Coordinate(40.72109533886229, -74.006596745813);
            this.startRideLocation = coord;
            this.endRideLocation = coord;
            this.rideDistance = 0.1;
            this.rideAddress = '50 Varick St, New York, NY 10012';

            this.bingMapsKey = 'YOUR_BING_MAPS_TOKEN_KEY';
            this.mobileServiceKey = 'YOUR_MOBILE_SERVICE_KEY';
            this.gcmSenderId = 'YOUR_GOOGLE_CLOUD_MESSAGING_ID';
            this.realTimeNotificationsServerUrl = 'http://YOUR_SITE.azurewebsites.net/';

            return this;
        }

        public getMobileServiceUrl = function () {
            return this.storageService.getValue('serviceUrl', 'http://YOUR_SITE.azure-mobile.net/');
        };
    }

    angularModule.factory('settingsService', (storageService) => {
        return new MyShuttle.Core.SettingsService(storageService);
    });
}