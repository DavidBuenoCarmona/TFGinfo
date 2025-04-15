export const route = {
    admin: {
        department: {
            create: '/admin/department/create',
            edit: '/admin/department/edit',
            list: '/admin/department',
        },
        university: {
            create: '/admin/university/create',
            edit: '/admin/university/edit',
            list: '/admin/university',
        },
        user: {
            create: '/admin/student/create',
            edit: '/admin/student/edit',
            list: '/admin/student',
        },
        career: {
            create: '/admin/career/create',
            edit: '/admin/career/edit',
            list: '/admin/career',
        },
    },
    booking: '/',
    login: '/login',
    professor: {
        create: '/professor/create',
        edit: '/professor/edit',
        list: '/professor',
    },
    profile: '/profile',
    workingGroup: {
        create: '/working-group/create',
        edit: '/working-group/edit',
        list: '/working-group',
    },
    tfg: {
        create: '/tfg/create',
        edit: '/tfg/edit',
        list: '/tfg',
    }
}