const ROLES = {
  USER: 'User',
  ADMIN: 'Admin'
};

const PERMISSIONS = {
  [ROLES.USER]: ['read'],            
  [ROLES.ADMIN]: ['read', 'write', 'delete'] 
};

export default {
  ROLES,
  PERMISSIONS
};