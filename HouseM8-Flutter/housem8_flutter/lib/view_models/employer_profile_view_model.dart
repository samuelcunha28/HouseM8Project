import 'package:flutter/cupertino.dart';
import 'package:housem8_flutter/models/employer_update.dart';
import 'package:housem8_flutter/models/profile_model.dart';
import 'package:housem8_flutter/services/employer_profile_service.dart';

class EmployerProfileViewModel extends ChangeNotifier {
  ProfileModel employerProfile;

  fetchEmployerProfile() async {
    this.employerProfile =
        await EmployerProfileService().fetchEmployerProfile();
    notifyListeners();
  }

  Future<void> updateEmployerProfile([EmployerUpdate employer]) async {
    employerProfile.firstName = employer.firstName;
    employerProfile.lastName = employer.lastName;
    employerProfile.userName = employer.userName;
    employerProfile.address = employer.address.toString();
    employerProfile.description = employer.description;
    notifyListeners();
    await EmployerProfileService().updateEmployer(employer);
  }

  int get id {
    return this.employerProfile.id;
  }

  String get firstName {
    return this.employerProfile.firstName;
  }

  set firstName(String firstName) {
    this.employerProfile.firstName = firstName;
  }

  String get lastName {
    return this.employerProfile.lastName;
  }

  set lastName(String lastName) {
    this.employerProfile.lastName = lastName;
  }

  String get userName {
    return this.employerProfile.userName;
  }

  set userName(String userName) {
    this.employerProfile.userName = userName;
  }

  String get address {
    return this.employerProfile.address;
  }

  set address(String address) {
    this.employerProfile.address = address;
  }

  String get description {
    if (this.employerProfile.description != null) {
      return this.employerProfile.description;
    } else {
      return "Sem descrição";
    }
  }

  set description(String description) {
    this.employerProfile.description = description;
  }

  double get averageRating {
    return this.employerProfile.averageRating;
  }
}
